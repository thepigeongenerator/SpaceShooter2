using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter2.Src.Data;
internal struct Astroid {
    public Transform transform = new();
    public bool unbreakable = false;

    public Astroid() {
    }
}
