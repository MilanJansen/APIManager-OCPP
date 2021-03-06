﻿using System;
using System.Windows.Forms;
using Framework.Logging;
using Framework.Context;

namespace Plugin.Application.Forms
{
    internal partial class SettingsForm : Form
    {
        private string _logfileName;
        private string _rootPath;
        private string _imageType;
        private string _interfaceType;

        /// <summary>
        /// the SettingsForm works in close cooperation with the FrameworkSettings component (accessed via Context) and is used to
        /// update FrameworkSettings in persistent storage through a user interface.
        /// </summary>
        internal SettingsForm()
        {
            InitializeComponent();
            ContextSlt context                      = ContextSlt.GetContextSlt();

            UseLogfile.Checked                      = context.GetBoolSetting(FrameworkSettings._UseLogFile);
            this._logfileName                       = context.GetStringSetting(FrameworkSettings._LogFileName);
            this._rootPath                          = context.GetStringSetting(FrameworkSettings._RootPath);
            this._imageType                         = context.GetStringSetting(FrameworkSettings._DiagramSaveType);
            this._interfaceType                     = context.GetStringSetting(FrameworkSettings._InterfaceContractType);

            CLAddCodeTypesToDiagram.Checked         = context.GetBoolSetting(FrameworkSettings._CLAddCodeTypesToDiagram);
            CLAddSourceEnumsToDiagram.Checked       = context.GetBoolSetting(FrameworkSettings._CLAddSourceEnumsToDiagram);
            AutoIncrementBuildNr.Checked            = context.GetBoolSetting(FrameworkSettings._AutoIncrementBuildNumbers);
            SMAddMessageAssemblyToDiagram.Checked   = context.GetBoolSetting(FrameworkSettings._SMAddMessageAssemblyToDiagram);
            SMAddBusinessMsgToDiagram.Checked       = context.GetBoolSetting(FrameworkSettings._SMAddBusinessMsgToDiagram);
            SMCreateCmnSchema.Checked               = context.GetBoolSetting(FrameworkSettings._SMCreateCommonSchema);
            SMUseMsgHeaders.Checked                 = context.GetBoolSetting(FrameworkSettings._SMUseMessageHeaders);
            SMUseSecurityLevels.Checked             = context.GetBoolSetting(FrameworkSettings._SMUseSecurityLevels);
            DEBusinessTermName.Checked              = context.GetBoolSetting(FrameworkSettings._DEBusinessTermName);
            DEDefinition.Checked                    = context.GetBoolSetting(FrameworkSettings._DEDefinition);
            DEDictionaryEntryName.Checked           = context.GetBoolSetting(FrameworkSettings._DEDictionaryEntryName);
            DENotes.Checked                         = context.GetBoolSetting(FrameworkSettings._DENotes);
            DEUniqueID.Checked                      = context.GetBoolSetting(FrameworkSettings._DEUniqueID);
            SaveMsgDiagrams.Checked                 = context.GetBoolSetting(FrameworkSettings._SaveMessageDiagrams);
            DocGenUseCommon.Checked                 = context.GetBoolSetting(FrameworkSettings._DocGenUseCommon);

            // Find out which radio button to select for diagram...
            foreach (Control control in DiagramTypes.Controls)
            {
                if (control is RadioButton && control.Text == this._imageType)
                {
                    ((RadioButton)control).Checked = true;
                    break;
                }
            }

            // Find out which radio button to select for interface type...
            foreach (Control control in InterfaceContracts.Controls)
            {
                if (control is RadioButton && (string)control.Tag == this._interfaceType)
                {
                    ((RadioButton)control).Checked = true;
                    break;
                }
            }

            LogfileName.Text    = this._logfileName;
            RootPathName.Text   = this._rootPath;
        }

        /// <summary>
        /// Invoked on OK event, indicating that the current settings must be transferred to configuration.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void Ok_Click(object sender, EventArgs e)
        {
            Logger.WriteInfo("Custom.Event.EventImp.SettingsForm.SettingsForm.Ok_Click >> We're done!");
            ContextSlt context = ContextSlt.GetContextSlt();
            context.FrameworkStartTransaction();

            context.SetBoolSetting(FrameworkSettings._UseLogFile, UseLogfile.Checked);
            context.SetStringSetting(FrameworkSettings._LogFileName, this._logfileName);
            context.SetBoolSetting(FrameworkSettings._CLAddCodeTypesToDiagram, CLAddCodeTypesToDiagram.Checked);
            context.SetBoolSetting(FrameworkSettings._CLAddSourceEnumsToDiagram, CLAddSourceEnumsToDiagram.Checked);
            context.SetBoolSetting(FrameworkSettings._AutoIncrementBuildNumbers, AutoIncrementBuildNr.Checked);
            context.SetBoolSetting(FrameworkSettings._SMAddMessageAssemblyToDiagram, SMAddMessageAssemblyToDiagram.Checked);
            context.SetBoolSetting(FrameworkSettings._SMAddBusinessMsgToDiagram, SMAddBusinessMsgToDiagram.Checked);
            context.SetBoolSetting(FrameworkSettings._SMCreateCommonSchema, SMCreateCmnSchema.Checked);
            context.SetBoolSetting(FrameworkSettings._SMUseMessageHeaders, SMUseMsgHeaders.Checked);
            context.SetBoolSetting(FrameworkSettings._SMUseSecurityLevels, SMUseSecurityLevels.Checked);
            context.SetBoolSetting(FrameworkSettings._DEBusinessTermName, DEBusinessTermName.Checked);
            context.SetBoolSetting(FrameworkSettings._DEDefinition, DEDefinition.Checked);
            context.SetBoolSetting(FrameworkSettings._DEDictionaryEntryName, DEDictionaryEntryName.Checked);
            context.SetBoolSetting(FrameworkSettings._DENotes, DENotes.Checked);
            context.SetBoolSetting(FrameworkSettings._DEUniqueID, DEUniqueID.Checked);
            context.SetBoolSetting(FrameworkSettings._SaveMessageDiagrams, SaveMsgDiagrams.Checked);
            context.SetStringSetting(FrameworkSettings._DiagramSaveType, this._imageType);
            context.SetStringSetting(FrameworkSettings._InterfaceContractType, this._interfaceType);
            context.SetBoolSetting(FrameworkSettings._DocGenUseCommon, DocGenUseCommon.Checked);

            // Check root path, should not end with separator (should not happen since user can not type the path, just to be save...)
            if (this._rootPath.EndsWith("/") || this._rootPath.EndsWith("\\"))
                this._rootPath = this._rootPath.Substring(0, this._rootPath.Length - 1);
            context.SetStringSetting(FrameworkSettings._RootPath, this._rootPath);

            // Check is we still have to use the logfile. If not, we switch to an empty filename, which effectively disables logging...
            // Note that empty files will not be persisted!
            if (UseLogfile.Checked) context.SetLogfile(this._logfileName);
            else context.SetLogfile(string.Empty);
            context.FrameworkCommit();
        }

        /// <summary>
        /// Invoked on CANCEL event, does not perform any operation (i.e. leaves all settings the way they were).
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            Logger.WriteInfo("Custom.Event.EventImp.SettingsForm.Cancel_Click >> We're giving up!");
        }

        /// <summary>
        /// Invoked when the user selected the 'logfile chooser' button.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void SelectLogfile_Click(object sender, EventArgs e)
        {
            if (LogfileSelector.ShowDialog() == DialogResult.OK)
            {
                this._logfileName = LogfileSelector.FileName;
                LogfileName.Text = this._logfileName;
                LogfileName.Update();
                Logger.WriteInfo("Custom.Event.EventImp.SettingsForm.SelectLogfile_Click >> Selected logfile: " + this._logfileName);
            }
        }

        /// <summary>
        /// Invoked when the user selected the 'root path chooser' button.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void SelectRootPath_Click(object sender, EventArgs e)
        {
            if (RootPathSelector.ShowDialog() == DialogResult.OK)
            {
                this._rootPath = RootPathSelector.SelectedPath;
                RootPathName.Text = this._rootPath;
                RootPathName.Update();
                Logger.WriteInfo("Custom.Event.EventImp.SettingsForm.SelectRootPath_Click >> Selected path: " + this._rootPath);
            }
        }

        /// <summary>
        /// We invoke this handler whenever one of the Diagram Types radio buttons is clicked.
        /// The function loads the string value of the currently selected radio button.
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">Ignored</param>
        private void DiagramTypes_CheckedChanged(object sender, EventArgs e)
        {
            foreach(Control control in DiagramTypes.Controls)
            {
                if (control is RadioButton && ((RadioButton)control).Checked)
                {
                    this._imageType = control.Text;
                    break;
                }
            }
        }

        /// <summary>
        /// We invoke this handler whenever one of the Interface Types radio buttons is clicked.
        /// Since string values for these buttons are meant as indicators for the user and not 
        /// su much usable from code, a textual Tag is defined per button. We use the tag
        /// value as labels.
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">Ignored</param>
        private void InterfaceContracts_CheckedChanges(object sender, EventArgs e)
        {
            foreach (Control control in InterfaceContracts.Controls)
            {
                if (control is RadioButton && ((RadioButton)control).Checked)
                {
                    this._interfaceType = control.Tag as string;
                    break;
                }
            }
        }

        /// <summary>
        /// This event is raised whenever the Add Message Assembly to Diagram changes state.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void SMCheckMSGAssemblyDependencies(object sender, EventArgs e)
        {
            if (SMAddMessageAssemblyToDiagram.Checked) SMAddBusinessMsgToDiagram.Checked = true;
        }

        /// <summary>
        /// This event is raised whenever the Add Business Message to Diagram changes state.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void SMCheckBusinessMsgDependencies(object sender, EventArgs e)
        {
            if (!SMAddBusinessMsgToDiagram.Checked) SMAddMessageAssemblyToDiagram.Checked = false;
        }
    }
}
