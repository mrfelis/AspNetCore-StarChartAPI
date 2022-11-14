﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarChart.Models
{
    public class CelestialObject
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int? OrbitedObjectId { get; set; }

        public List<CelestialObject> Satelites { get; set; }

        public TimeSpan OrbitalPeriod { get; set; }
    }
}
