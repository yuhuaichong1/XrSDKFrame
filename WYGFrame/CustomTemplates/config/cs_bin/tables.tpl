using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Bright.Serialization;
using UnityEngine.Networking;
using System.IO;
using XrCode;

{{
    name = x.name
    namespace = x.namespace
    tables = x.tables
	tblCount = x.tables.Count
}}
namespace {{namespace}}
{
   
	public partial class {{name}}
	{
		{{~for table in tables ~}}
		{{~if table.comment != '' ~}}
		/// <summary>
		/// {{table.escape_comment}}
		/// </summary>
		{{~end~}}
		public {{table.full_name}} {{table.name}} {get; private set;}
	{{~end~}}

		private Queue<string> configNames;
		private Queue<System.Action<ByteBuf>> configCbFuncs;
		private System.Func<string, ByteBuf> _loader;
		private System.Action finishHandle;
		private string ServerResourceURL = "http://192.168.30.35:5005/";

		private System.Collections.Generic.Dictionary<string, object> tables;

		public {{name}}(System.Func<string, ByteBuf> loader)
		{
			_loader = loader;
			tables = new System.Collections.Generic.Dictionary<string, object>();
			{{~for table in tables ~}}
			{{table.name}} = new {{table.full_name}}(loader("{{table.output_data_file}}")); 
			tables.Add("{{table.full_name}}", {{table.name}});
			{{~end~}}
	
			PostInit();
			ResolveAllTable();
			PostResolve();
		}

	    public Tables(System.Action cb)
        {
			finishHandle = cb;
            configNames = new Queue<string>();
            configCbFuncs = new Queue<System.Action<ByteBuf>>();
		    tables = new System.Collections.Generic.Dictionary<string, object>();
			{{~for table in tables ~}}
			configNames.Enqueue("{{table.output_data_file}}");
            configCbFuncs.Enqueue(On{{table.name}}DataFinish);
			{{~end~}}

            LoadAllConfig();
        }

        public void LoadAllConfig()
        {
            if (configNames.Count == 0)
            {
                OnLoadTbDataFinish();
                return;
            }
            string configName = configNames.Dequeue();
            System.Action<ByteBuf> cb = configCbFuncs.Dequeue();
            Game.Instance.StartCoroutine(WebLoad(configName, cb,LoadAllConfig));
        }

        public IEnumerator WebLoad(string fileName, System.Action<ByteBuf> confInst, System.Action cb)
        {
            Debug.LogError($"[ConfPath]:Begin load config {fileName}");
            //string path = StringUtil.Concat(ServerResourceURL, $"StreamingAssets/Data/{fileName}.bytes");
            string path = StringUtil.Concat(Application.streamingAssetsPath, $"/Data/{fileName}.bytes");
			
            //if (!File.Exists(path)) Debug.LogError($" config path is wrong. {fileName} ");

            UnityWebRequest request = UnityWebRequest.Get(path);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                confInst(new ByteBuf(request.downloadHandler.data));
                cb();
            }
            else
            {
                Debug.LogError($"[ConfPath]: UnityWebRequest Load fail: {request.result}");
            }
        }

		public void TranslateText(System.Func<string, string, string> translator)
		{
			{{~for table in tables ~}}
			{{table.name}}.TranslateText(translator); 
			{{~end~}}
		}
		
		partial void PostInit();
		partial void PostResolve();
	
		private void ResolveAllTable()
		{
			{{~for table in tables ~}}
			{{table.name}}.Resolve(tables);
			{{~end~}}
		}
	
		private void ReloadOneTable(string reloadTableName)
		{
			if (!tables.Keys.Contains(reloadTableName))
			{
				return;
			}
	
			switch (reloadTableName)
			{
				{{~for table in tables ~}}
				case "{{table.full_name}}":
					{{table.name}}.Reload(_loader("{{table.full_name}}"));
					break;
				{{~end~}}
			}
	
		}
	
		public void Reload(params string[] reloadTableNames)
		{
			foreach (var reloadTableName in reloadTableNames)
			{
				ReloadOneTable(reloadTableName);
			}
			ResolveAllTable();
		}
		
	
		public void ReloadAll()
		{
			Reload(tables.Keys.ToArray());
		}

    		    {{~for table in tables ~}}
	{{~if table.comment != '' ~}}
		/// <summary>
		/// {{table.escape_comment}}
		/// </summary>
	{{~end~}}
		public void On{{table.name}}DataFinish(ByteBuf buf)
		{
			{{table.name}} = new {{table.full_name}}(buf);
			tables.Add("{{table.full_name}}", {{table.name}});
		}
		{{~end~}}
		//Finish Load all table 
		public void OnLoadTbDataFinish()
		{
			PostInit();
			ResolveAllTable();
			PostResolve();
			finishHandle?.Invoke();
		}

	}

}