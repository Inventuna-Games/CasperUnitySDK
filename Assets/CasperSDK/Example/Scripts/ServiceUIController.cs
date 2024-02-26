using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using System.Linq;
using UnityEngine.UI;
using System;
using CasperSDK;
using CasperSDK.DataStructures;
using CasperSDK.WalletData;
using CasperSDK.Auth;
using CasperSDK.NFT;

public class ServiceUIController : MonoBehaviour
{
    [SerializeField]
    GameObject AuthPanel;
     [SerializeField]
    GameObject InformationPanel;
    [SerializeField]
    GameObject InformationPanelLoading;

    [SerializeField]
    TMP_Text WalletAdress;
    
    [SerializeField]
    TMP_Text Tokens;
    
    [SerializeField]
    TMP_Text NFTs;
    
    [SerializeField]
    TMP_Text Balance;


    #region NFT

    [SerializeField]
    TMP_InputField TransferTokenIDInput;
    [SerializeField]
    TMP_InputField TransferTargetWalletInput;
    [SerializeField]
    TMP_InputField BurnTokenIDInput;
    [SerializeField]
    TMP_InputField MintPackageHashInput;
    [SerializeField]
    TMP_InputField MintMetadataInput;
    [SerializeField]
    TMP_InputField RegisterOwnerTokenIDInput;

    #endregion

    #region CEP18

    [SerializeField]
    TMP_InputField TransferCEP18ContractHashInput;
    [SerializeField]
    TMP_InputField TransferCEP18AmountInput;
    [SerializeField]
    TMP_InputField TransferCEP18TargetWalletInput;
    [SerializeField]
    TMP_InputField BurnCEP18ContractHashInput;
    [SerializeField]
    TMP_InputField BurnCEP18AmountInput;
    [SerializeField]
    TMP_InputField MintCEP18ContractHashInput;
    [SerializeField]
    TMP_InputField MintCEP18AmountInput;
    [SerializeField]
    TMP_InputField ApproveCEP18ContractHashInput;
    [SerializeField]
    TMP_InputField ApproveCEP18AmountInput;
    #endregion
    
    

    void Start()
    {
        OpenAuthPanel();
        AuthManager.Instance.OnAuthRespond += OpenInformationPanel;
        WalletDataManager.Instance.OnWalletInformationUpdated += UpdateInformationPanel;
        NFTManager.Instance.OnTransferRespond += OnTransferSuccess;
    }

    public void StartTransferNFT()
    {
        NFTInformation NFTInfo = WalletDataManager.Instance.CurrentAuthorizedWalletInformation.walletNFTs.Find(x => x.token_id == TransferTokenIDInput.text);
        string TargetWallet = TransferTargetWalletInput.text;
        ServiceManager.Instance.StartTransferNFT(NFTInfo ,TargetWallet);
    }
    public void StartBurnNFT()
    {
        NFTInformation NFTInfo = WalletDataManager.Instance.CurrentAuthorizedWalletInformation.walletNFTs.Find(x => x.token_id == BurnTokenIDInput.text);
        ServiceManager.Instance.StartBurnNFT(NFTInfo);
    }
    public void StartMintNFT()
    {
        string Metadata = MintMetadataInput.text;
        string PackageHash = MintPackageHashInput.text;
        ServiceManager.Instance.StartMintNFT(PackageHash ,Metadata);
    }
    public void StartRegisterOwnerNFT()
    {
        NFTInformation NFTInfo = WalletDataManager.Instance.CurrentAuthorizedWalletInformation.walletNFTs.Find(x => x.token_id == RegisterOwnerTokenIDInput.text);
        ServiceManager.Instance.StartRegisterOwnerNFT(NFTInfo);
    }
    public void StartTransferCEP18()
    {
        string TargetWallet = TransferCEP18TargetWalletInput.text;
        string ContractHash = TransferCEP18ContractHashInput.text;
        string Amount = TransferCEP18AmountInput.text;
        ServiceManager.Instance.StartTransferCEP18(float.Parse(Amount), ContractHash,TargetWallet);
    }
    public void StartBurnCEP18()
    {
        string ContractHash = BurnCEP18ContractHashInput.text;
        string Amount = BurnCEP18AmountInput.text;
        ServiceManager.Instance.StartBurnCEP18(float.Parse(Amount),ContractHash);
    }
    public void StartMintCEP18()
    {
        string ContractHash = MintCEP18ContractHashInput.text;
        string Amount = MintCEP18AmountInput.text;
        ServiceManager.Instance.StartMintCEP18(float.Parse(Amount) ,ContractHash);
    }
    public void StartApproveCEP18()
    {
        string ContractHash = ApproveCEP18ContractHashInput.text;
        string Amount = ApproveCEP18AmountInput.text;
        ServiceManager.Instance.StartApproveCEP18(float.Parse(Amount),ContractHash);
    }
    void CloseAuthPanel()
    {
        AuthPanel.SetActive(false);
    }
    void OpenAuthPanel()
    {
        InformationPanel.SetActive(false);
        SetInformationPanelLoading(false);
        AuthPanel.SetActive(true);
    }
    void OpenInformationPanel(ServiceResponse Respond)
    {
        if(Respond.status == RequestStatus.Success)
        {
            InformationPanel.SetActive(true);
            SetInformationPanelLoading(true);
        }
        CloseAuthPanel();
        ServiceManager.Instance.UpdateWalletInformation();
    }
    
    void UpdateInformationPanel(WalletInformation walletInformation)
    {
        WalletAdress.SetText(walletInformation.walletPublicKey);
        

        string  TokenString = "";
        foreach (var item in walletInformation.walletTokens)
        {
            TokenString += " // " + item.balance +  " "+ item.contract_package.metadata.symbol;
        }
        Tokens.SetText(TokenString);

        string NFTString = "";
        foreach (var item in walletInformation.walletNFTs)
        {
            NFTString += " // " + item.token_id;
        }
        NFTs.SetText(NFTString);
        Balance.SetText(walletInformation.walletBalance.ToString() + " CSPR");
        SetInformationPanelLoading(false);
        AuthPanel.SetActive(false);
    }
    void OnTransferSuccess(ServiceResponse Respond)
    {
        if(Respond.status == RequestStatus.Success)
        {
            ServiceManager.Instance.UpdateWalletInformation();
        }
    }
    void SetInformationPanelLoading(bool isLoading)
    {
        InformationPanelLoading.SetActive(isLoading);
    }
}
