

namespace My
{

    // 注意:這是自動產生的檔案，請勿直接修改它。若要進行變更，
    // 或者您在這個檔案發生建置錯誤，請到專案設計工具
    // (移至專案屬性或者在 [方案總管] 中按兩下 [My Project] 節點)，
    // 然後在 [應用程式] 索引標籤上進行變更。
    // 
    internal partial class MyApplication
    {
        [global::System.Diagnostics.DebuggerStepThrough()]
        public MyApplication() : base(global::Microsoft.VisualBasic.ApplicationServices.AuthenticationMode.Windows)
        {
            this.IsSingleInstance = false;
            this.EnableVisualStyles = true;
            this.SaveMySettingsOnExit = true;
            this.ShutDownStyle = global::Microsoft.VisualBasic.ApplicationServices.ShutdownMode.AfterMainFormCloses;
        }

        [global::System.Diagnostics.DebuggerStepThrough()]
        protected override void OnCreateMainForm()
        {
            this.MainForm = global::OVG.MainForm;
        }
    }
}

