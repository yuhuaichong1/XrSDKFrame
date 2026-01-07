using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using XrCode;

public class GM : MonoBehaviour
{
    public Button GMBtn;
    public GameObject GMPlane;
    [Space]
    public Button MoneyBtn;
    public InputField MoneyField;
    [Space]
    public Button EnergyBtn;
    public InputField EnergyField;
    [Space]
    public Button PropBtn;
    public Dropdown PropDropdown;

    private bool GMbool;

    void Awake()
    {
        GMbool = false;

        if (GMBtn != null && GMPlane != null)
        {
            GMPlane.gameObject.SetActive(GMbool);
            GMBtn.onClick.AddListener(OnGMBtnClick);
        }
            
        if (MoneyBtn != null && MoneyField != null)
        {
            MoneyBtn.onClick.AddListener(OnMoneyBtnClick);
        }

        if(EnergyBtn != null && EnergyField != null)
        {
            EnergyBtn.onClick.AddListener(OnEnergyBtnClick);
        }

        if (PropBtn != null && PropDropdown != null)
        {
            PropBtn.onClick.AddListener(OnPropBtnClick);
        }
    }

    private void OnGMBtnClick()
    {
        GMbool = !GMbool;
        GMPlane.gameObject.SetActive(GMbool);
    }

    private void OnMoneyBtnClick()
    {
        double money;
        if(double.TryParse(MoneyField.text, out money))
        {
            FacadePlayer.AddMoney(money);
            FacadeGamePlay.SetCurMoneyShow();
        }
    }

    private void OnEnergyBtnClick()
    {
        int energy;
        if(int.TryParse(EnergyField.text, out energy))
        {
            FacadePlayer.AddEnergy(energy);
            FacadeGamePlay.SetCurEnergyShow(EShowEnergyType.All);
        }
        

    }

    private void OnPropBtnClick()
    {
        switch(PropDropdown.value) 
        {
            case 0:
                FacadePlayer.AddProp1Num(1);
                FacadeGamePlay.SetProp1CountShow();
                break;
            case 1:
                FacadePlayer.AddProp2Num(1);
                FacadeGamePlay.SetProp2CountShow();
                break;
            case 2:
                FacadePlayer.AddProp3Num(1);
                FacadeGamePlay.SetProp3CountShow();
                break;
        }
    }

}
