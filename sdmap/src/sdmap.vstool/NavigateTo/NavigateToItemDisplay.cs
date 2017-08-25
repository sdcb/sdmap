using Microsoft.VisualStudio.Language.NavigateTo.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Diagnostics;
using EnvDTE;

namespace sdmap.Vstool.NavigateTo
{
    internal sealed class NavigateToItemDisplay : INavigateToItemDisplay
    {
        public string Name { get; }

        public System.Drawing.Icon Glyph => null;

        public string AdditionalInformation => "sdmap";

        public string Description => null;

        public NavigateToMatch Match { get; set; }

        public ReadOnlyCollection<DescriptionItem> DescriptionItems =>
            new ReadOnlyCollection<DescriptionItem>(new List<DescriptionItem>());

        public NavigateToItemDisplay(NavigateToItem item)
        {
            Match = (NavigateToMatch)item.Tag;
            Name = Match.MatchedText;
        }

        public void NavigateTo()
        {
            if (!Match.ProjectItem.IsOpen)
            {
                var window = Match.ProjectItem.Open();
                window.Visible = true;
                window.Activate();
                window.DTE.ExecuteCommand("Edit.Goto", Match.Start.Line.ToString());
            }
        }
    }
}
