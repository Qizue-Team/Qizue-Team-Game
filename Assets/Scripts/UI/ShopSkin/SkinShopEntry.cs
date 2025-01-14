using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkinShopEntry : MonoBehaviour
{
    public static event Action<Skin> OnSkinSet;

    [Header("References")]
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI costText;
    [SerializeField]
    private Button unlockSetButton;
    [SerializeField]
    private Button tryOnButton;

    private Skin _skin;
    private bool _addOnceUnlock = false;
    private bool _addOnceSet = false;
    private bool _addOnceTryOn = false;
    private bool _addOnceUnset = false;

    private void OnEnable()
    {
        OnSkinSet += UnsetSkinFromOtherSet;
    }

    private void OnDisable()
    {
        OnSkinSet -= UnsetSkinFromOtherSet;
    }

    public void SetEntry(Skin skin)
    {
        _skin = skin;
        iconImage.sprite = skin.gameObject.GetComponent<SpriteRenderer>().sprite;
        nameText.text = skin.SkinName;
        costText.text = "Gear Cost: " + skin.Cost;

        // Listeners here
        if(!_skin.IsUnlocked && !_addOnceUnlock)
        {
            _addOnceUnlock = true;
            unlockSetButton.onClick.RemoveAllListeners();
            unlockSetButton.onClick.AddListener(() => Unlock());
        }
        else if (_skin.IsUnlocked && !_skin.IsSet && !_addOnceSet)
        {
            _addOnceSet = true;
            unlockSetButton.onClick.RemoveAllListeners();
            unlockSetButton.onClick.AddListener(() => SetSkin());
            unlockSetButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "S E T";
        }
        else if (_skin.IsUnlocked && _skin.IsSet && !_addOnceUnset)
        {
            _addOnceUnset = true;
            unlockSetButton.onClick.RemoveAllListeners();
            unlockSetButton.onClick.AddListener(() => UnsetSkin());
            unlockSetButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "U N S E T";
        }

        if (!_addOnceTryOn)
        {
            _addOnceTryOn= true;
            tryOnButton.onClick.RemoveAllListeners();
            tryOnButton.onClick.AddListener(()=>TryOn());
        }
    }

    // TO-DO: Listener methods here Unlock and Try On
    private void Unlock()
    {
        if (_skin.IsUnlocked)
            return;
        _skin.Unlock();
        FindObjectOfType<GearText>().UpdateGearText(DataManager.Instance.LoadTotalGearCount());

        // Call method for the Manager -> it will save data persistently
        SkinShopManager.Instance.Unlock();

        // Now it's unlocked so add set
        if (_skin.IsUnlocked && !_addOnceSet)
        {
            _addOnceSet = true;
            unlockSetButton.onClick.RemoveAllListeners();
            unlockSetButton.onClick.AddListener(() => SetSkin());
            unlockSetButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "S E T";
        }
    }

    private void TryOn()
    {
        // TryOn code
        FindObjectOfType<SkinAttachPoint>().TrySkin(_skin);
    }

    private void SetSkin()
    {
        if (_skin.IsSet)
            return;


        OnSkinSet?.Invoke(_skin);

        // Set code
        FindObjectOfType<SkinAttachPoint>().SetSkin(_skin);


        // Now it's unset
        if(_skin.IsUnlocked && _skin.IsSet)
        {
            unlockSetButton.onClick.RemoveAllListeners();
            unlockSetButton.onClick.AddListener(() => UnsetSkin());
            unlockSetButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "U N S E T";
        }

        // Set code
        FindObjectOfType<SkinAttachPoint>().SetSkin(_skin);
    }

    private void UnsetSkin()
    {
        // Unset code
        FindObjectOfType<SkinAttachPoint>().UnsetSkin(_skin);

        // Now it's unlocked so add set
        if (!_skin.IsSet)
        {
            unlockSetButton.onClick.RemoveAllListeners();
            unlockSetButton.onClick.AddListener(() => SetSkin());
            unlockSetButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "S E T";
        }
    }

    private void UnsetSkinFromOtherSet(Skin skin)
    {
        if (skin.ID == _skin.ID)
            return;
        if (!_skin.IsSet)
            return;
        UnsetSkin();
    }
}
