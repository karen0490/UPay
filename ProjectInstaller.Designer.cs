namespace UPay
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.UPayServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.UPayServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // UPayServiceProcessInstaller
            // 
            this.UPayServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.UPayServiceProcessInstaller.Password = null;
            this.UPayServiceProcessInstaller.Username = null;            
            // 
            // UPayServiceInstaller
            // 
            this.UPayServiceInstaller.Description = "UPayService para pago de transacciones con tarjetas";
            this.UPayServiceInstaller.DisplayName = "UPayService";
            this.UPayServiceInstaller.ServiceName = "UPayService";
            this.UPayServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.UPayServiceProcessInstaller,
            this.UPayServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller UPayServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller UPayServiceInstaller;
    }
}