﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TheDogWalkingApp2.Models
{
   public class Walks
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public int WalkerId { get; set; }
        public Walker Walker { get; set; }
        public int DogId { get; set; }
        public Dog Dog { get; set; }
    }
}
