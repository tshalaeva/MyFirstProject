﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Entity
    {
        public int Id { get; protected set; }

        public Entity()
        {
        }

        public Entity(int id)
        {
            Id = id;
        }
    }
}
