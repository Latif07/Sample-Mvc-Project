using System;

namespace SampleWebProject
{

    public static class Constants
    {
        public const int AllItemId = -1;
        public const string AllItemName = "All";
        public const int NewItemId = 0;
        public const int MaxRecordCount = 500;
        public const string DateFormat = "dd.MM.yyyy";
        public const string DateTimeFormat = "dd.MM.yyyy hh:MM";
        public static readonly DateTime DefaultMinDateTime = new DateTime(1900, 1, 1);
        public static readonly DateTime DefaultMaxDateTime = new DateTime(2099, 12, 31);
        public const string DefaultCulture = "tr-TR";
    }
}