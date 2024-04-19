﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace storage_managements
{
    class lib_comboBox
    {
        public static void add_items(ComboBox cb, List<DS_Company> items)
        {
            foreach (DS_Company item in items)
            {
                cb.Items.Add(item.name);
            }    
        }
    }
}