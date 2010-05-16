using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SmartyPantsPivotLibrary
{
    /// <summary>
    /// This class is associated with menus in the stack panel across the top of the pivot, since there is an endless list of menu entries
    /// some of these are duplicates, so we use these entries to keep track of what the real menus are for the fake ones - and we will
    /// instantly jump the user to a real one without them realising it, thus keeping the endless menu going across the top
    /// </summary>
    public class MenuTitleEntry
    {
        public string Title { get; private set; }
        public FrameworkElement PageContentReference { get; private set; }
        public MenuTitleEntry RealMenuPointer { get; private set; } // may be a pointer to itself for real menu entries

        public MenuTitleEntry(PivotPage pivotPage)
        {
            Title = pivotPage.PageTitle;
            PageContentReference = pivotPage.Content as FrameworkElement;
            RealMenuPointer = this;
        }

        public MenuTitleEntry(MenuTitleEntry realMenu)
        {
            Title = realMenu.Title;
            PageContentReference = realMenu.PageContentReference;
            RealMenuPointer = realMenu;
        }
    }
}
