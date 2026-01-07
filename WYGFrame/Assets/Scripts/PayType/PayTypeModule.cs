using cfg;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace XrCode
{
    public class PayTypeModule : BaseModule
    {
        List<PayNode> payItems;//可用支付类型

        private string mark;//货币单位
        private int decimals;//显示暴保留小数点
        private float exchangeRate;//汇率
        private int NANP;//国际长途电话区号
        private string countryCode;//国家码
        private string Language;//语言

        protected override void OnLoad()
        {
            base.OnLoad();

            payItems = new List<PayNode>();

            FacadePayType.GetPayItems += GetPayItems;
            FacadePayType.RegionalChange += RegionalChange;
            FacadePayType.GetNANP += GetNANP;
            FacadePayType.GetCountryCode += GetCountryCode;
            FacadePayType.GetLanguage += GetLanguage;

            CountryCodeToInfo();
        }


        //根据国家码获取语言和支付信息
        private void CountryCodeToInfo()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            countryCode = currentCulture.Name.Split("-")[1];
            List<ConfPayRegion> payRegionList = ConfigModule.Instance.Tables.TBPayRegion.DataList;
            foreach (ConfPayRegion payRegion in payRegionList) 
            {
                if (payRegion.Code == countryCode)
                {
                    string[] pays = payRegion.Channels.Split(',');
                    GetPayTypes(pays);

                    mark = payRegion.Mark;
                    decimals = payRegion.Decimal;
                    exchangeRate = payRegion.ExchangeRate;

                    Language = payRegion.Lang;
                    ModuleMgr.Instance.LanguageMod.SetLanguage((ELanguageType)Enum.Parse(typeof(ELanguageType), payRegion.Lang));

                    NANP = payRegion.NANP;

                    return;
                }
            }

            //默认情况下取值
            GetDefinePayType();

        }

        /// <summary>
        /// 默认情况下取值
        /// </summary>
        private void GetDefinePayType()
        {
            string[] defPays = GameDefines.Default_Channels.Split(',');
            GetPayTypes(defPays);
            mark = GameDefines.Default_Mark;
            decimals = GameDefines.Default_Decimal;
            exchangeRate = GameDefines.Default_ExchangeRate;
            ModuleMgr.Instance.LanguageMod.SetLanguage(GameDefines.Default_Language);
            NANP = GameDefines.Default_NANP;
            Language = GameDefines.Default_Language2;
        }

        /// <summary>
        /// 得到该地区的支付信息
        /// </summary>
        /// <param name="pays">支付方式</param>
        private void GetPayTypes(string[] pays)
        {
            foreach (string pay in pays) 
            {
                ConfPayChannel pc = ConfigModule.Instance.Tables.TBPayChannel.Get(int.Parse(pay));
                payItems.Add(new PayNode
                {
                    payType = (EPayType)pc.Sn,
                    infoType = (EPOEType)pc.InfoType,
                    picture = ResourceMod.Instance.SyncLoad<Sprite>(pc.PicPath),
                    icon = ResourceMod.Instance.SyncLoad<Sprite>(pc.IconPath),
                });
            }
        }

        /// <summary>
        /// 获取支付类型
        /// </summary>
        /// <returns>支付类型</returns>
        private List<PayNode> GetPayItems()
        {
            return payItems;
        }

        /// <summary>
        /// 将值以汇率的方式显示
        /// </summary>
        /// <param name="value">目前钱数（以 $ 为例）</param>
        /// <returns>更新或的值</returns>
        private string RegionalChange(double value)
        {
            if (!GameDefines.ifIAA)
                return value.CurrencyConvertV2(mark, ",", 3, decimals);
            else
                return $"{(int)value}";
        }

        /// <summary>
        /// 获取国际长途电话区号
        /// </summary>
        /// <returns>国际长途电话区号</returns>
        private int GetNANP()
        {
            return NANP;
        }

        /// <summary>
        /// 获取国家码
        /// </summary>
        /// <returns>国家码</returns>
        private string GetCountryCode()
        {
            return countryCode;
        }

        /// <summary>
        /// 获取语言
        /// </summary>
        /// <returns>语言</returns>
        private string GetLanguage()
        {
            return Language;
        }


        protected override void OnDispose()
        {
            payItems = null;

            FacadePayType.GetPayItems -= GetPayItems;
            FacadePayType.RegionalChange -= RegionalChange;
            FacadePayType.GetNANP -= GetNANP;
            FacadePayType.GetCountryCode -= GetCountryCode;
            FacadePayType.GetLanguage -= GetLanguage;

            //GetDefinePayType();
        }
    }
}


