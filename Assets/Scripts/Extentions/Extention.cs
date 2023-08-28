using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Extentions
{
    public static class Extention
    {
        #region Vector
        public static Vector3 MultiplyX(this Vector3 v, float val)
        {
            v = new Vector3(val * v.x, v.y, v.z);
            return v;
        }
        public static Vector3 MultiplyY(this Vector3 v, float val)
        {
            v = new Vector3(v.x, val * v.y, v.z);
            return v;
        }
        public static Vector3 MultiplyZ(this Vector3 v, float val)
        {
            v = new Vector3(v.x, v.y, val * v.z);
            return v;
        }

        public static Vector2 Vector3ToVector2(Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }
        public static Vector3 Vector2ToVector3(Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }
        #endregion

        #region Components
        public static T GetOrAddComponent<T>(this GameObject child) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.AddComponent<T>();
            }
            return result;
        }

        public static GameObject SetName(this GameObject gameObject, string name)
        {
            gameObject.name = name;
            return gameObject;
        }
        public static GameObject AddRigidBody2D(this GameObject gameObject, float mass)
        {
            var component = gameObject.GetOrAddComponent<Rigidbody2D>();
            component.mass = mass;
            return gameObject;
        }
        public static GameObject AddRigidBody(this GameObject gameObject, float mass)
        {
            var component = gameObject.GetOrAddComponent<Rigidbody>();
            component.mass = mass;
            return gameObject;
        }
        public static GameObject AddBoxCollider2D(this GameObject gameObject)
        {
            var component = gameObject.GetOrAddComponent<BoxCollider2D>();
            return gameObject;
        }
        public static GameObject AddBoxCollider(this GameObject gameObject)
        {
            var component = gameObject.GetOrAddComponent<BoxCollider>();
            return gameObject;
        }
        public static GameObject AddCircleCollider2D(this GameObject gameObject)
        {
            var component = gameObject.GetOrAddComponent<CircleCollider2D>();
            return gameObject;
        }
        public static GameObject AddSphereCollider(this GameObject gameObject)
        {
            var component = gameObject.GetOrAddComponent<SphereCollider>();
            return gameObject;
        }
        public static GameObject AddSprite(this GameObject gameObject, Sprite sprite)
        {
            var component = gameObject.GetOrAddComponent<SpriteRenderer>();
            component.sprite = sprite;
            return gameObject;
        }
        public static GameObject AddMesh(this GameObject gameObject, Material material)
        {
            var component = gameObject.GetOrAddComponent<MeshRenderer>();
            component.material = material;
            return gameObject;
        }
        #endregion

        #region Random

        public static double GetRandomDouble(double minimum, double maximum)
        { 
            var random = new System.Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
        
        public static float GetRandomFloat(double minimum, double maximum)
        { 
            var random = new System.Random();
            return (float)(random.NextDouble() * (maximum - minimum) + minimum);
        }

        public static int GetRandomInt(int minimum, int maximum)
        {
            var random = new System.Random();
            return random.Next(minimum, maximum);
        }

        #endregion
        
        #region String
        
            public static List<string> SplitBy(this string str, int chunkLength)
            {
                List<string> strings = new List<string>();
                if (String.IsNullOrEmpty(str)) throw new ArgumentException();
                if (chunkLength < 1) throw new ArgumentException();

                for (int i = 0; i < str.Length; i += chunkLength)
                {
                    if (chunkLength + i > str.Length)
                        chunkLength = str.Length - i;

                    strings.Add(str.Substring(i, chunkLength));
                }

                return strings;
            }

        #endregion
    }
}