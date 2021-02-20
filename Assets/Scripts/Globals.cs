using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static class Animation
    {
        public static readonly string Flash = "Flash";
        public static readonly string IsAttacking = "isAttacking";
        public static readonly string IsJumping = "isJumping";
        public static readonly string MoveX = "moveX";
    }

    public static class Input
    {
        public static readonly string Attack = "Attack";
        public static readonly string Glide = "Glide";
        public static readonly string Goop = "Goop";
        public static readonly string Jump = "Jump";
        
        public static readonly string Horizontal = "Horizontal";
        public static readonly string Vertical = "Vertical";

    }

    public static class Tags
    {
        public static readonly string Player = "Player";
    }
}
