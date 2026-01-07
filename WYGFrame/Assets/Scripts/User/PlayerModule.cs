using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace XrCode
{
    // 用户模块
    public class PlayerModule : BaseModule
    {
        #region 玩家数据

        private double money;//当前金币
        private int diamond;//当前钻石

        private int energy;//当前体力

        private int level;//当前关卡

        private int prop1Num;//添加空间道具数量
        private int prop2Num;//清除道具数量
        private int prop3Num;//锤子道具数量

        private string userName;//玩家姓名
        private string userID;//玩家ID

        private int userLevel;//当前等级
        private int userExp;//当前经验

        #endregion

        private STimer recoverEnergyTimer;
        private DateTime lastRecoverTime;

        protected override void OnLoad()
        {
            base.OnLoad();
            FacadeAdd();
            LoadData();
            StartRecoverEnergyTimer();
        }

        #region Facade

        private void FacadeAdd()
        {
            FacadePlayer.GetMoney += GetMoney;
            FacadePlayer.SetMoney += SetMoney;
            FacadePlayer.AddMoney += AddMoney;

            FacadePlayer.GetDiamond += GetDiamond;
            FacadePlayer.SetDiamond += SetDiamond;
            FacadePlayer.AddDiamond += AddDiamond;

            FacadePlayer.GetEnergy += GetEnergy;
            FacadePlayer.AddEnergy += AddEnergy;
            FacadePlayer.GetCurRemainingTime += GetCurRemainingTime;

            FacadePlayer.GetLevel += GetLevel;
            FacadePlayer.SetLevel += SetLevel;
            FacadePlayer.AddLevel += AddLevel;

            FacadePlayer.GetProp1Num += GetProp1Num;
            FacadePlayer.SetProp1Num += SetProp1Num;
            FacadePlayer.AddProp1Num += AddProp1Num;

            FacadePlayer.GetProp2Num += GetProp2Num;
            FacadePlayer.SetProp2Num += SetProp2Num;
            FacadePlayer.AddProp2Num += AddProp2Num;

            FacadePlayer.GetProp3Num += GetProp3Num;
            FacadePlayer.SetProp3Num += SetProp3Num;
            FacadePlayer.AddProp3Num += AddProp3Num;

            FacadePlayer.GetPlayerName += GetUserName;

            FacadePlayer.GetPlayerID += GetUserID;

            FacadePlayer.GetPlayerLevel += GetUserLevel;

            FacadePlayer.GetPlayerExp += GetUserExp;
            FacadePlayer.AddPlayerExp += AddUserExp;
        }

        private void FacadeRemove() 
        {
            FacadePlayer.GetMoney -= GetMoney;
            FacadePlayer.SetMoney -= SetMoney;
            FacadePlayer.AddMoney -= AddMoney;

            FacadePlayer.GetDiamond -= GetDiamond;
            FacadePlayer.SetDiamond -= SetDiamond;
            FacadePlayer.AddDiamond -= AddDiamond;

            FacadePlayer.GetEnergy -= GetEnergy;
            FacadePlayer.AddEnergy -= AddEnergy;

            FacadePlayer.GetLevel -= GetLevel;
            FacadePlayer.SetLevel -= SetLevel;
            FacadePlayer.AddLevel -= AddLevel;

            FacadePlayer.GetProp1Num -= GetProp1Num;
            FacadePlayer.SetProp1Num -= SetProp1Num;
            FacadePlayer.AddProp1Num -= AddProp1Num;

            FacadePlayer.GetProp2Num -= GetProp2Num;
            FacadePlayer.SetProp2Num -= SetProp2Num;
            FacadePlayer.AddProp2Num -= AddProp2Num;

            FacadePlayer.GetProp3Num -= GetProp3Num;
            FacadePlayer.SetProp3Num -= SetProp3Num;
            FacadePlayer.AddProp3Num -= AddProp3Num;

            FacadePlayer.GetPlayerName -= GetUserName;

            FacadePlayer.GetPlayerID -= GetUserID;

            FacadePlayer.GetPlayerLevel -= GetUserLevel;

            FacadePlayer.GetPlayerExp -= GetUserExp;
            FacadePlayer.AddPlayerExp -= AddUserExp;
        }

        #endregion

        #region Get/Set

        #region money

        private double GetMoney()
        {
            return money;
        }

        private void SetMoney(double value)
        {
            money = value;
            SPlayerPrefs.SetDouble(PlayerPrefDefines.money, money);
            SPlayerPrefs.Save();
        }

        private void AddMoney(double value)
        {
            money += value;
            SPlayerPrefs.SetDouble(PlayerPrefDefines.money, money);
            SPlayerPrefs.Save();
        }

        #endregion

        #region diamond

        private int GetDiamond()
        {
            return diamond;
        }

        private void SetDiamond(int value)
        {
            diamond = value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.diamond, diamond);
            SPlayerPrefs.Save();
        }

        private void AddDiamond(int value)
        {
            diamond += value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.diamond, diamond);
            SPlayerPrefs.Save();
        }

        #endregion

        #region energy

        private int GetEnergy()
        {
            return energy;
        }

        private bool AddEnergy(int value)
        {
            energy += value;
            if(energy > GameDefines.Default_MaxEnergy) energy = GameDefines.Default_MaxEnergy;
            if(energy < 0)  energy = 0;

            SPlayerPrefs.SetInt(PlayerPrefDefines.energy, energy);
            SPlayerPrefs.Save();

            bool ifMax = energy == GameDefines.Default_MaxEnergy;
            CheckRecoverEnergyTimer(ifMax);

            return ifMax;
        }

        #endregion

        #region level

        private int GetLevel()
        {
            return level;
        }

        private void SetLevel(int value)
        {
            level = value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.level, level);
            SPlayerPrefs.Save();
        }

        private void AddLevel(int value)
        {
            level += value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.level, level);
            SPlayerPrefs.Save();
        }

        #endregion

        #region prop1Num

        private int GetProp1Num()
        {
            return prop1Num;
        }

        private void SetProp1Num(int value)
        {
            prop1Num = value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.prop1Num, prop1Num);
            SPlayerPrefs.Save();
        }

        private void AddProp1Num(int value)
        {
            prop1Num += value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.prop1Num, prop1Num);
            SPlayerPrefs.Save();
        }

        #endregion

        #region prop2Num

        private int GetProp2Num()
        {
            return prop2Num;
        }

        private void SetProp2Num(int value)
        {
            prop2Num = value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.prop2Num, prop2Num);
            SPlayerPrefs.Save();
        }

        private void AddProp2Num(int value)
        {
            prop2Num += value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.prop2Num, prop2Num);
            SPlayerPrefs.Save();
        }

        #endregion

        #region prop3Num

        private int GetProp3Num()
        {
            return prop3Num;
        }

        private void SetProp3Num(int value)
        {
            prop3Num = value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.prop3Num, prop3Num);
            SPlayerPrefs.Save();
        }

        private void AddProp3Num(int value)
        {
            prop3Num += value;
            SPlayerPrefs.SetInt(PlayerPrefDefines.prop3Num, prop3Num);
            SPlayerPrefs.Save();
        }

        #endregion

        #region userName

        private string GetUserName()
        {
            return userName;
        }

        #endregion

        #region userID

        private string GetUserID()
        {
            return userID;
        }

        #endregion

        #region userLevel
        
        private int GetUserLevel()
        {
            return userLevel;
        }

        #endregion

        #region userExp

        private int GetUserExp()
        {
            return userExp;
        }

        private void AddUserExp(int value)
        {
            if (userLevel >= ConfigModule.Instance.Tables.TBUserLevel.DataList.Count - 1)
            {
                D.Log("max level");
                return;
            }

            userExp += value;
            CheckNextUserLevel();
            SPlayerPrefs.SetInt(PlayerPrefDefines.userLevel, userLevel);
            SPlayerPrefs.SetInt(PlayerPrefDefines.userExp, userExp);
            SPlayerPrefs.Save();
        }

        #endregion

        #endregion

        #region 其他

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            money = SPlayerPrefs.GetDouble(PlayerPrefDefines.money, 0);
            level = SPlayerPrefs.GetInt(PlayerPrefDefines.level, 0);
            energy = SPlayerPrefs.GetInt(PlayerPrefDefines.energy, GameDefines.Default_MaxEnergy);
            prop1Num = SPlayerPrefs.GetInt(PlayerPrefDefines.prop1Num, GameDefines.Default_Prop1_Count);
            prop2Num = SPlayerPrefs.GetInt(PlayerPrefDefines.prop2Num, GameDefines.Default_Prop2_Count);
            prop3Num = SPlayerPrefs.GetInt(PlayerPrefDefines.prop3Num, GameDefines.Default_Prop3_Count);
            userName = SPlayerPrefs.HasKey(PlayerPrefDefines.userName) ? SPlayerPrefs.GetString(PlayerPrefDefines.userName) : GetRandomName();
            userID = SPlayerPrefs.HasKey(PlayerPrefDefines.userID) ? SPlayerPrefs.GetString(PlayerPrefDefines.userID) : GetRandomID();
            userLevel = SPlayerPrefs.GetInt(PlayerPrefDefines.userLevel, 0);
            userExp = SPlayerPrefs.GetInt(PlayerPrefDefines.userExp, 0);
        }

        /// <summary>
        /// 获取随机玩家姓名
        /// </summary>
        /// <returns>随机玩家姓名</returns>
        private string GetRandomName()
        {
            char[] nameChars = GameDefines.NameString.ToCharArray();
            int length = nameChars.Length;
            char c1 = nameChars[UnityEngine.Random.Range(0, length)];
            char c2 = nameChars[UnityEngine.Random.Range(0, length)];
            string target = $"{FacadeLanguage.GetText("10005")}_{c1}{c2}";

            SPlayerPrefs.SetString(PlayerPrefDefines.userName, target);
            SPlayerPrefs.Save();

            return target;
        }

        /// <summary>
        /// 获取随机玩家ID
        /// </summary>
        /// <returns>随机玩家ID</returns>
        private string GetRandomID()
        {
            string target = "";
            Guid GID = Guid.NewGuid();
            target = GID.ToString();

            SPlayerPrefs.SetString(PlayerPrefDefines.userID, target);
            SPlayerPrefs.Save();

            return target;
        }

        /// <summary>
        /// 检测玩家是否升级
        /// </summary>
        private void CheckNextUserLevel()
        {
            int nextExp = ConfigModule.Instance.Tables.TBUserLevel.Get(userLevel).NextLvNeedExp;
            if (userExp >= nextExp)
            {
                userLevel += 1;
                userExp -= nextExp;
                CheckNextUserLevel();
            }
        }

        /// <summary>
        /// 开局检测体力并创建相关计时器
        /// </summary>
        private void StartRecoverEnergyTimer()
        {
            double timeDifference = (DateTime.UtcNow - lastRecoverTime).TotalSeconds;
            if(timeDifference < 0)
                timeDifference = 0;

            int addEnergy = (int)(timeDifference / GameDefines.Default_Energy_RecoverTime);
            if (addEnergy < 0) 
                addEnergy = 0;

            bool ifMax = AddEnergy(addEnergy);

            int remainingTime = (int)(GameDefines.Default_Energy_RecoverTime - timeDifference % GameDefines.Default_Energy_RecoverTime);

            recoverEnergyTimer = STimerManager.Instance.CreateSTimer(remainingTime, -1, false, false, () =>
            {
                recoverEnergyTimer.targetTime = GameDefines.Default_Energy_RecoverTime;

                lastRecoverTime = DateTime.UtcNow;
                SPlayerPrefs.SetDateTime(PlayerPrefDefines.lastRecoverTime, lastRecoverTime);
                //SPlayerPrefs.Save();//在Addenergy中有Save

                if (AddEnergy(GameDefines.Default_Energy_TimeAdd))
                    recoverEnergyTimer.Stop();
            });

            if (ifMax)
            {
                recoverEnergyTimer.targetTime = GameDefines.Default_Energy_RecoverTime;
                recoverEnergyTimer.Stop();
            }

        }

        /// <summary>
        /// 获取体力恢复剩余时间
        /// </summary>
        /// <returns>剩余时间（秒）</returns>
        private int GetCurRemainingTime()
        {
            if (recoverEnergyTimer == null)
                return -1;

            int curRemainingTime = 0;
            curRemainingTime = (int)(recoverEnergyTimer.targetTime - recoverEnergyTimer.nowTime);

            return curRemainingTime;
        }

        /// <summary>
        /// 体力减少时，是否开启计时器
        /// </summary>
        private void CheckRecoverEnergyTimer(bool ifMax)
        {
            if (recoverEnergyTimer == null || ifMax)
                return;
            
            recoverEnergyTimer.Start();
        }

        #endregion

        protected override void OnDispose()
        {
            base.OnDispose();

            FacadeRemove();
        }
    }
}