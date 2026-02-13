using System;
using SQLite4Unity3d;

namespace Domain
{
    public class FileData
    {
        [PrimaryKey] public string Path { get; set; }
    }
}