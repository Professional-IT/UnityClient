using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global 
{
    public static string DOMAIN = "localhost:3000";
    public static User m_user;
}

public class User { 
    public long id;
    public string name;
    public long score;
}
