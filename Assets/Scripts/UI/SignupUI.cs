using UnityEngine;
using UnityEngine.UI;

public class SignupUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Button Btn_Exit;
    [SerializeField] InputField Input_Id;
    [SerializeField] InputField Input_Password;
    [SerializeField] Button Btn_Signup;
    [SerializeField] Text Text_Error;

    private void OnEnable()
    {
        Btn_Exit.onClick.AddListener(OnClick_Exit);
        Btn_Signup.onClick.AddListener(OnClick_Signup);

        bool DBConnected = SignupManager.Instance.SignupUI_OnEnable_ConnectDB();
        if (DBConnected) { Text_Error.text = "DB 연결 성공!"; }
        else { Text_Error.text = "DB 연결 실패!"; }
    }

    private void OnDisable()
    {
        Btn_Exit.onClick.RemoveListener(OnClick_Exit);
        Btn_Signup.onClick.RemoveListener(OnClick_Signup);
        
        bool DBDisconnected = SignupManager.Instance.SignupUI_OnDisable_DisconnectDB();
    }

    public void OnClick_Exit()
    {
        SignupManager.Instance.SignupUI_OnClick_Exit();
    }

    public void OnClick_Signup()
    {
        bool signupSuccess = SignupManager.Instance.SignupUI_OnClick_Signup_SendQuery(Input_Id.text, Input_Password.text);
        if (signupSuccess) { Text_Error.text = "회원가입 성공!"; }
        else { Text_Error.text = "회원가입 실패!"; }
    }

}
