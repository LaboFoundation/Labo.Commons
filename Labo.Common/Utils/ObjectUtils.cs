﻿using System;
using System.Globalization;

namespace Labo.Common.Utils
{
    public static class ObjectUtils
    {
        public static TObject Cast<TObject>(object @object)
        {
            return (TObject) @object;
        }

        public static string ToStringInvariant(object @object)
        {
            return Convert.ToString(@object, CultureInfo.InvariantCulture);
        }
    }
}
