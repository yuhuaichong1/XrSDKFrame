/* 
 * 游戏定义脚本
 * 游戏中常量、枚举等内容请在这里统一定义
 */

using UnityEngine;

public abstract class GameDefines
{
    #region 常量

    #region 打包相关
    public static string URL = "http://www.gamelajk.xyz/xgame?appIndex=16";                                 //后台链接网址
    public static bool ifIAA = false;                                                                       //是否为IAA模式
    public static bool ifDebug = true;                                                                      //是否是debug模式
    public static bool ifSkipAD = true;                                                                     //是否跳过广告
    #endregion

    #region 游戏相关
    public static string NameString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";               //随机名称数组

    public static float Elimination_Money = 0.02f;                                                          //单次消除金额
    public static float IAA_Elimination_Money = 1f;                                                         //单次消除金额（IAA）
  
    #endregion

    #region 特效相关

    public static float GRE_StayTime = 1f;                                                                  //获取奖励特效持续时间（同时也是之后的飞物体特效的延迟时间）
    public static int GRE_FlyMoneyCount = 8;                                                                //奖励特效执行飞行钱特效时飞钱的数量
    public static float LTE_MoveTime = 0.5f;                                                                //关卡目标特效移入时间
    public static float LTE_StayTime = 2f;                                                                  //关卡目标特效持续时间
    public static float LTE_GoAwayTime = 0.5f;                                                              //关卡目标特效移出时间
    public static float CE_MoveTime = 0.2f;                                                                 //祝贺特效移入时间
    public static float CE_StayTime = 1f;                                                                   //祝贺特效持续时间
    public static Vector2 CE_Content_attemptTimes = new Vector2(1, 3);                                      //祝贺特效内容随机尝试次数
    public static float FlyProp_MoveTime = 0.5f;                                                            //飞行道具持续时间
    public static int FlyMoney_FlyMoneyCount = 8;                                                           //单次消除后飞行钱特效的数量
    public static float FlyMoney_MoveTime = 0.5f;                                                           //飞行钱持续时间
    public static float FlyMoney_DelayTime = 0.5f;                                                          //飞行钱延迟播放时间
    public static float FlyMoney_IntervalTime = 0.1f;                                                       //飞行钱间隔时间
    public static float FlyMoney_RandomSpawnDist = 100f;                                                    //飞行钱随机生成位置的区间
    public static float FlyMoneyTip_DelayTime = 1f;                                                         //飞行钱提示延迟播放时间
    public static float FlyMoneyTip_MoveTime = 2f;                                                          //飞行钱提示持续时间
    public static float FlyMoneyTip_MoveDist = 60f;                                                         //飞行钱提示移动距离
    public static float DifficultyUp_StayTime = 1;                                                          //难度提升特效持续时间
    public static Vector2 TargetArriveTime = new Vector2(1, 5);                                             //关卡目标特效随机时间

    #endregion

    #region 兑现相关

    public static float[] Withdrawal_Quota = new float[6]                                                   //可兑现金额固定区间
    {3000, 4000, 5000, 6000, 7000, 8000};
    public static Vector2 Withdrawal_RQuota = new Vector2(1000, 2000);                                      //可兑现金额随机区间1
    public static int[] WithdrawalLevels = new int[3] { 1, 2, 6 };                                          //可兑现关卡
    public static float MinWithdrawalAmount = 4000;                                                         //最低兑现金额
    public static Vector2 WithdrawalAmountRandomTime = new Vector2(1f, 3f);                                 //兑现进度界面随机延迟显示时间

    #endregion

    #region UI打开、关闭动画时间

    public static float ShowAnimTime = 0.25f;                                                               //UI动画打开持续时间
    public static float HideAnimTime = 0.25f;                                                               //UI动画关闭持续时间

    #endregion

    #region 默认语言&支付方式及其相关

    public static string Default_Channels = "0";                                                            //默认支付方式
    public static string Default_Mark = "$";                                                                //默认货币符号
    public static int Default_Decimal = 2;                                                                  //默认小数点位
    public static int Default_ExchangeRate = 1;                                                             //默认汇率
    public static ELanguageType Default_Language = ELanguageType.English;                                   //默认语言
    public static int Default_NANP = 1;                                                                     //默认国际长途电话区号
    public static string Default_Language2 = "English";                                                     //默认语言（标题用）

    #endregion

    #region 下三道具功能默认数量

    public static int Default_Prop1_Count = 3;                                                              //道具1“A”默认数量
    public static int Default_Prop2_Count = 3;                                                              //道具2“B”默认数量
    public static int Default_Prop3_Count = 3;                                                              //道具3“C”默认数量

    #endregion

    #region 各种路径

    #region 奖励特效小图标路径
    public static string ERMoneyIconPath = "UI/RewardEffect/icon_qianbidui.png";                            //三叠钱
    public static string ERIAAMoneyIconPath = "UI/LuckySpinIcons/icon_qianbi_IAA.png";                      //一叠硬币
    public static string ERAddSpaceIconPath = "UI/FuncIcon/SFuncIcon_Prop1.png";                            //添加空间道具
    public static string ERClearIconPath = "UI/FuncIcon/SFuncIcon_Prop2.png";                               //清除道具
    public static string ERHammerIconPath = "UI/FuncIcon/SFuncIcon_Prop3.png";                              //锤子道具
    #endregion

    #endregion

    #region 玩家相关
    public static int Default_MaxEnergy = 5;                                                                //默认体力
    public static float Default_Energy_RecoverTime = 600;                                                   //默认体力恢复时间（秒）
    public static int Default_Energy_TimeAdd = 1;                                                           //计时结束后所能恢复的体力

    public static int EXP_Single = 2;                                                                       //单次消除获取的经验
    #endregion

    #region 新手引导相关
    public static int firstGuideId = 10001;                                                                 //第一次新手引导步骤                                             
    #endregion

    #region 幸运奖励界面相关
    public static int LuckyReward_CheckCount = 8;                                                           //每完成X次条件，弹一次弹窗
    public static Vector2 LuckyReward_RandomRange = new Vector2(30f, 50f);                                  //奖励区间
    #endregion

    #endregion
}

#region 枚举

/// <summary>
/// 游戏状态
/// </summary>
public enum EGameState
{
    Load,             //加载游戏
    Start,            //开始游戏
    Run,              //运行游戏
    Pause,            //暂停游戏
    Exit,             //退出游戏
}

/// <summary>
/// UI类型枚举  和ui表配置一一对应
/// </summary>
public enum EUIType
{
    ENone = 0,
    EUIGamePlay = 1,
    EUILoading = 2,
    EUINotice = 3,
    EUIGuide = 4,
    EUIEffect = 5,
    EUISetting = 6,
    EUIUserLevel = 7,
    EUIReStart = 8,
    EUILevelCompleted = 9,
    EUILevelFailure = 10,
    EUIProp = 11,
    EUIEnterInfomation = 12,
    EUIFeedback = 13,
    EUIConfirm = 14,
    EUIWithdrawalRecords = 15,
    EUIWithdrawalAmount = 16,
    EUILuckyReward = 17,
}

/// <summary>
/// 场景类型枚举
/// </summary>
public enum ESceneType : byte
{
    MainScene = 1,          //主城场景
    Meun = 2,               //主菜单      
}

/// <summary>
/// UI状态
/// </summary>
public enum EUIState
{
    EUIHiding,
    EUIOpened,
    EUIClosed,
}

/// <summary>
/// 监听接口
/// </summary>
public enum EMsgCode : int
{
    EC2S_Login = 1001, //登录
    ES2C_Login = 1002,
}

/// <summary>
/// 多语言
/// </summary>
public enum ELanguageType : int
{
    None = 0,
    Chinese_s = 1,
    Chinese_t = 2,
    English = 3,
    German = 4,
    Japanese = 5,
    Brazilian_Portuguese = 6,
    French = 7,
    Spanish = 8,
    Korean = 9,
    Indonesian = 10,
    Russian = 11,
    Hindi = 12,
    Thai = 13,
    Turkish = 14,
    Arabic = 15,

    LengthTest = 99,
}

/// <summary>
/// 下三功能
/// </summary>
public enum EFuncType : int
{
    Prop1 = 0,
    Prop2 = 1,
    Prop3 = 2,
}

/// <summary>
/// 广告来源
/// </summary>
public enum EAdSource
{
    Prop,//道具界面
    LevelCompleted,//关卡结算
    LevelFailureTryAgain,//关卡失败重来
    LuckyReward,//幸运一刻
}

/// <summary>
/// 广告类型
/// </summary>
public enum EAdType
{
    Reward,//激励
    Interstitial,//插屏
    Banner,//横幅
}

/// <summary>
/// 支付类型（与表格PayChannel的sn一一对应）
/// </summary>
public enum EPayType : int
{
    PayPal = 0,
    Venmo = 1,
    Zelle = 2,
    E_Transfer = 3,
    Mercado = 4,
    BBVA = 5,
    Pix = 6,
    PicPay = 7,
    Skrill = 8,
    Revolut = 9,
    Sofort = 10,
    Paylib = 11,
    PayPay = 12,
    Other = 13,
}

/// <summary>
/// 支付所填写的信息的类型（与表格PayChannel的infoType一一对应）
/// </summary>
public enum EPOEType : int
{  
    Email = 1,//邮箱
    Phone = 2,//电话
    POE = 3,//电话or邮箱
}

/// <summary>
/// 兑现订单状态
/// </summary>
public enum EWOrderState
{
    Processing,
    Finish,
    Error,
}

/// <summary>
/// 奖励类型
/// </summary>
public enum ERewardType
{
    Money = 0,
    Prop1 = 1,
    Prop2 = 2,
    Prop3 = 3,
}

/// <summary>
/// 提现订单类型
/// </summary>
public enum EWithRecordState : int
{
    GoWithdrawal = 0,//去提款
    UnderReview = 1,//审核中
}

/// <summary>
/// 需要刷新的体力的部分
/// </summary>
public enum EShowEnergyType
{
    Energy,
    Time,
    All,
}

#endregion

#region 结构体

/// <summary>
/// 奖励类型特效用结构体
/// </summary>
public struct ERewardItemStruct
{
    public ERewardType Type;
    public float Count;
}

#endregion
