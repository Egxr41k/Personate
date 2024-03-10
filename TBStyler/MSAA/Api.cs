using System;
using System.Runtime.InteropServices;
using System.Text;
using Accessibility;

namespace TBStyler.MSAA;

public class Api
{
    public static Guid guidAccessible = new Guid("{618736E0-3C3D-11CF-810C-00AA00389B71}");

    public static IAccessible[] GetAccessibleChildren(IAccessible objAccessible)
    {
        int childCount;
        try
        {
            childCount = objAccessible.accChildCount;
        }
        catch (Exception)
        {
            childCount = 0;
        }

        IAccessible[] accObjects = new IAccessible[childCount];
        int count = 0;

        if (childCount != 0)
        {
            Intertop.AccessibleChildren(objAccessible, 0, childCount, accObjects, out count);
        }

        return accObjects;
    }

    public static IAccessible GetAccessibleFromHandle(nint hwnd)
    {
        object accObject = new object();
        IAccessible objAccessible = null;

        if (hwnd != nint.Zero)
        {
            Intertop.AccessibleObjectFromWindow(Convert.ToInt32(hwnd), 0, ref guidAccessible, out accObject);
            objAccessible = (IAccessible)accObject;
        }

        return objAccessible;
    }

    public static string GetStateTextFunc(uint stateID)
    {
        uint maxLength = 1024;
        StringBuilder focusableStateText = new StringBuilder((int)maxLength);
        StringBuilder sizeableStateText = new StringBuilder((int)maxLength);
        StringBuilder moveableStateText = new StringBuilder((int)maxLength);
        StringBuilder invisibleStateText = new StringBuilder((int)maxLength);
        StringBuilder pressedStateText = new StringBuilder((int)maxLength);
        StringBuilder hasPopupStateText = new StringBuilder((int)maxLength);

        if (stateID == (Intertop.STATE_SYSTEM_INVISIBLE | Intertop.STATE_SYSTEM_FOCUSABLE | Intertop.STATE_SYSTEM_HASPOPUP))
        {
            Intertop.GetStateText(Intertop.STATE_SYSTEM_INVISIBLE, invisibleStateText, maxLength);
            Intertop.GetStateText(Intertop.STATE_SYSTEM_FOCUSABLE, focusableStateText, maxLength);
            Intertop.GetStateText(Intertop.STATE_SYSTEM_HASPOPUP, hasPopupStateText, maxLength);

            return $"{invisibleStateText},{focusableStateText},{hasPopupStateText}";
        }

        if (stateID == (Intertop.STATE_SYSTEM_PRESSED | Intertop.STATE_SYSTEM_INVISIBLE | Intertop.STATE_SYSTEM_FOCUSABLE))
        {
            Intertop.GetStateText(Intertop.STATE_SYSTEM_PRESSED, pressedStateText, maxLength);
            Intertop.GetStateText(Intertop.STATE_SYSTEM_INVISIBLE, invisibleStateText, maxLength);
            Intertop.GetStateText(Intertop.STATE_SYSTEM_PRESSED, focusableStateText, maxLength);

            return $"{pressedStateText},{focusableStateText},{focusableStateText}";
        }

        if (stateID == (Intertop.STATE_SYSTEM_FOCUSABLE | Intertop.STATE_SYSTEM_HASPOPUP))
        {
            Intertop.GetStateText(Intertop.STATE_SYSTEM_FOCUSABLE, focusableStateText, maxLength);
            Intertop.GetStateText(Intertop.STATE_SYSTEM_HASPOPUP, hasPopupStateText, maxLength);

            return $"{focusableStateText},{hasPopupStateText}";
        }

        if (stateID == Intertop.STATE_SYSTEM_FOCUSABLE)
        {
            Intertop.GetStateText(Intertop.STATE_SYSTEM_FOCUSABLE, focusableStateText, maxLength);
            return focusableStateText.ToString();
        }

        StringBuilder stateText = new StringBuilder((int)maxLength);
        Intertop.GetStateText(stateID, stateText, maxLength);
        return stateText.ToString();
    }

    public static RectangleX GetLocation(IAccessible acc, int idChild)
    {
        RectangleX rect = new RectangleX();
        if (acc != null)
        {
            acc.accLocation(out rect.left, out rect.top, out rect.width, out rect.height, idChild);
        }
        return rect;
    }
}
