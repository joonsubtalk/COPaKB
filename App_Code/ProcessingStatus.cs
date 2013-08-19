using System;
using System.Data;
using System.Configuration;

using System.Collections ;

/// <summary>
/// Summary description for ProcessingStatus
/// </summary>
public static class ProcessingStatus
{

    public static Hashtable m_Status= new Hashtable();
    public static object getValues(Guid itemID)
    {
        return m_Status[itemID]; 
    }

    public static void add(Guid itemID, object oStatus)
    {
        m_Status[itemID] = oStatus;
    }

    public static void update(Guid itemID, object oStatus)
    {
        m_Status[itemID] = oStatus; 
    }

    public static void remove(Guid itemID)
    {
        m_Status.Remove(itemID);
    }

    public static bool Contains(Guid itemID)
    {
        return m_Status.Contains(itemID);
    }
}
