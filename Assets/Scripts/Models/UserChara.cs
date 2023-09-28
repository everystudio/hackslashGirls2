using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class UserChara : CsvModelParam
{
    public int id;

    public int questPartyId;
    public int collectPartyId;

    public int partyIndex
    {
        get
        {
            return questPartyId > 0 ? questPartyId : collectPartyId;
        }
    }
}
