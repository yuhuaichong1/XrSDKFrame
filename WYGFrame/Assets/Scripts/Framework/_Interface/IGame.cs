using UnityEngine;
using System.Collections;

namespace XrCode
{
    /// <summary>
    /// 游戏进程逻辑接口
    /// </summary>
    public interface IGame
    {
        //游戏状态
        EGameState GameState { get; }
        //加载
        void Load();
        //运行
        void Update();
        //退出
        void Dispose();
        /// <summary>
        /// 注册游戏更新对象
        /// </summary>
        void RegisterUpdateObject(IUpdate updateObj);
        /// <summary>
        /// 注销游戏更新对象
        /// </summary>
        void UnRegisterUpdateObject(IUpdate updateObj);
    }
}