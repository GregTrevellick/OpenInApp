//using Microsoft.VisualStudio.Shell;
//using OpenInApp.Command;
//using OpenInApp.Common.Helpers;
//using OpenInApp.Menu;
//using OpenInAppPaintDotNet;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Windows.Forms;

//namespace OpenInPaintDotNet.Options.PaintDotNet
//{
//    public class OptionsNew : DialogPage, IGeneralOptionsFile // or set to IGeneralOptionsFolder
//    {
//        private const string CommonActualPathToExeOptionLabel = CommonConstants.ActualPathToExeOptionLabelPrefix + KeyToExecutableString.PaintDotNet;

//        [Category(CommonConstants.CategorySubLevel)]
//        [DisplayName(CommonActualPathToExeOptionLabel)]
//        [Description(CommonConstants.ActualPathToExeOptionDetailedDescription)]
//        public string ActualPathToExe { get; set; }

//        public override void LoadSettingsFromStorage()
//        {
//            base.LoadSettingsFromStorage();

//            if (string.IsNullOrEmpty(ActualPathToExe))
//            {
//                ActualPathToExe = GeneralOptionsHelper.GetActualPathToExe(keyToExecutableEnum);
//            }

//            previousActualPathToExe = ActualPathToExe;
//        }

//        private string previousActualPathToExe { get; set; }

//        protected override void OnApply(PageApplyEventArgs e)
//        {
//            var actualPathToExeChanged = false;

//            if (ActualPathToExe != previousActualPathToExe)
//            {
//                actualPathToExeChanged = true;
//                previousActualPathToExe = ActualPathToExe;
//            }

//            if (actualPathToExeChanged)
//            {
//                if (!ArtefactsHelper.DoesActualPathToExeExist(ActualPathToExe))
//                {
//                    e.ApplyBehavior = ApplyKind.Cancel;

//                    var caption = new ConstantsForAppCommon().Caption;

//                    var filePrompterHelper = new FilePrompterHelper(caption, keyToExecutableEnum.Description());

//                    var persistOptionsDto = filePrompterHelper.PromptForActualExeFile(ActualPathToExe);

//                    if (persistOptionsDto.Persist)
//                    {
//                        PersistVSToolOptions(persistOptionsDto.ValueToPersist);
//                    }
//                }
//            }

//            base.OnApply(e);
//        }

//        public void PersistVSToolOptions(string fileName)
//        {
//            VSPackage.Options.ActualPathToExe = fileName;
//            VSPackage.Options.SaveSettingsToStorage();
//        }
//    }
//}
