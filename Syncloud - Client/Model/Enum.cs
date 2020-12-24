using System;
using System.Collections.Generic;
using System.Text;

namespace Syncloud.Model
{
    public enum Status
    {
        //Up to date with distant server
        Synchronised,
        //Not pushed
        Ahead,
        //Not pulled
        Behind,
        //Folder does not exist online
        New,
        //Folder does not exist locally
        Deleted
    }
}
