using System;
using System.Collections.Generic;
using System.Linq;
namespace Task_1
{
    class Polynomial
    {
        private SortedDictionary<int,int> _coeffs;
        public Polynomial()
        {
            _coeffs=new SortedDictionary<int, int> ();
        }
        public Polynomial(params int[] array)
        {
            _coeffs=new SortedDictionary<int, int>();
            for (int i=0; i<array.Length; i++)
            {
                _coeffs.Add(array.Length-1-i, array[i]);
            }
        }
        public Polynomial (IDictionary<int,int> pairs)
        {
            foreach (int key in pairs.Keys)
            {
                if (key<0) 
                throw new Exception("Polynomials cannot contain negative variable exponents."); 
            }
            _coeffs=new SortedDictionary<int, int>(pairs);
        }

        public int Order
        {
            get=>Dict.Keys.Max();
        }
        public override string ToString()
        {
            string res="";
            foreach (int key in Dict.Keys)
            {
                if (Dict[key]<0)
                    res+=$"{Dict[key]}X^{key} ";
                else
                    res+=$"+{Dict[key]}X^{key} ";
            }
            return res;
        }

        public int this[int power]
        {
            get=>_coeffs[power];
            private set => _coeffs[power]=value;
        }

        private SortedDictionary<int,int> Dict
        {
            get=> _coeffs;
            set=> _coeffs=value;
        }

        public static Polynomial operator +(Polynomial expr1, Polynomial expr2)
        {
            Polynomial res= new Polynomial(expr1.Dict);
            foreach(int key in expr2.Dict.Keys)
            {
                if (res.Dict.ContainsKey(key))
                    res[key]+=expr2[key];
                else 
                    res[key]=expr2[key];
            }
            return res;
        }
        public static Polynomial operator -(Polynomial expr1, Polynomial expr2)
        {
            return expr1 + expr2*(-1);
        }

        public static Polynomial operator *(Polynomial expr, int num)
        {
            Polynomial res= new Polynomial ();
            foreach (int key in expr.Dict.Keys)
            {
                res[key]=expr[key]*num;
            }
            return res;
        }

        public static Polynomial operator * (int num, Polynomial expr)
        {
            return expr*num;
        }

        public static Polynomial operator * (Polynomial expr1, Polynomial expr2)
        {

            int low=expr1.Dict.Keys.Min()+expr2.Dict.Keys.Min();
            int high=expr1.Dict.Keys.Max()+expr2.Dict.Keys.Max();

            Polynomial res= new Polynomial();

            foreach (int key1 in expr1.Dict.Keys)
            {
                foreach (int key2 in expr2.Dict.Keys)
                {
                    if (res.Dict.ContainsKey(key1+key2))
                        res[key1+key2]+=expr1[key1]*expr2[key2];
                    else res[key1+key2]=expr1[key1]*expr2[key2];
                }
            }

            return res;
        }
        public Polynomial Power(int n)
        {
            Polynomial res= this*1;
            for (int i=1; i<n; i++)
            {
                res=res*this;
            }
            return res;
        } 
    }
}