using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPwork1
{

    enum Sex
    {
        M,
        F
    }

    class Person
    {
        public int childID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public Person Mother { get; set; }
        public Person Father { get; set; }
        private List<Person> Child { get; set; }
        private Person maritalUnion { get; set; }

        public Person(string name, Sex sex, int age )
        {
            Name = name;
            Sex = sex;
            Age = age;
            Child = new List<Person>();
        }
        public void SetParents(Person mother = null, Person father = null)
        {
            if (mother != null)
            {
                if (mother.Sex == Sex.F && mother.Age > Age)
                    Mother = mother;
                else
                    if (mother.Sex != Sex.F)
                    throw new ArgumentException(String.Format("Не тот пол"));
                if (mother.Age < Age)
                    throw new ArgumentException("Мать не может быть младше ребенка");
            }
            if (father != null)
            {
                if (father.Sex == Sex.M && father.Age > Age)
                    Father = father;
                else
                    if (father.Sex != Sex.M)
                    throw new ArgumentException(String.Format("Не тот пол"));
                if (father.Age < Age)
                    throw new ArgumentException("Отец не может быть младше ребенка");
            }

        }
        public string GetParents() => string.Format("{0} Является матерью {1}\n{2} Является отцом {1}", Mother.Name, Name, Father.Name);
        public void SetChild(Person child)
        {
            if (child != this)
                Child.Add(child);
            if (maritalUnion != null)
                maritalUnion.Child.Add(child);
            child.childID = Child.Count();
            if (Sex == Sex.M)
                child.SetParents(maritalUnion, this);
            else
                child.SetParents(this, maritalUnion);
        }

        public static void Wedding(Person mUnion1, Person mUnion2)
        {
            mUnion1.maritalUnion = mUnion2;
            mUnion2.maritalUnion = mUnion1;
        }

        private List<Person> GetSistersAndBrothersParent(Person grandma, Person grandpa) => grandma.Child.Union(grandpa.Child).ToList();

        public string GetCousin()
        {
            var result = "Двоюродные братья и сестры:\n";
            if (Mother != null)
            {
                var motherSistersAndBrothers = GetSistersAndBrothersParent(Mother.Mother, Mother.Father);
                result += "Двоюродные братья и сестры по матери:\n";
                foreach (var mSAB in motherSistersAndBrothers)
                {
                    if (mSAB.childID == Mother.childID)
                        continue;
                    foreach (var cousin in mSAB.Child)
                    {
                        result += string.Format("\t{0}, ребенок {1}\n", cousin.Name, mSAB.Name);
                    }
                }
            }
            if (Mother != null)
            {
                var fatherSistersAndBrothers = GetSistersAndBrothersParent(Father.Mother, Father.Father);
                result += "Двоюродные братья и сестры по отцу:\n";
                foreach (var mSAB in fatherSistersAndBrothers)
                {
                    if (mSAB.childID == Father.childID)
                        continue;
                    foreach (var cousin in mSAB.Child)
                    {
                        result += string.Format("\t{0}, ребенок {1}\n", cousin.Name, mSAB.Name);
                    }
                }
            }

            return result;
        }
        public string GetUncAlunt()
        {
            var result = "Дяди и Тети:\n";
            if (Mother.Mother != null && Mother.Father != null)
            {
                var motherSistersAndBrothers = GetSistersAndBrothersParent(Mother.Mother, Mother.Father);
                result += "Дяди и Тети по матери:\n";
                foreach (var mSAB in motherSistersAndBrothers)
                {
                    if (mSAB.childID == Mother.childID)
                        continue;
                    result += string.Format("\t{0}\n", mSAB.Name);
                }
            }
            if (Father.Mother != null && Father.Father != null)
            {
                var fatherSistersAndBrothers = GetSistersAndBrothersParent(Father.Mother, Father.Father);
                result += "Дяди и Тети по отцу:\n";
                foreach (var mSAB in fatherSistersAndBrothers)
                {
                    if (mSAB.childID == Mother.childID)
                        continue;
                    result += string.Format("\t{0}\n", mSAB.Name);
                }
            }
            return result;
        }

        public string GetInLaw() => $"Отец супруга(супруги): {maritalUnion.Father.Name}\n Мать супруга(супруги): {maritalUnion.Mother.Name}";


    }
    class Program
    {
        static void Main(string[] args)
        {
            var alex = new Person("Alex", Sex.M, 50);
            var alexa = new Person("Alexa", Sex.F, 40); 

            var albert = new Person("Albert", Sex.M, 20); 
            var sergey = new Person("Sergey", Sex.M, 35);
            var frank = new Person("Frank", Sex.M, 14);
            var anna = new Person("Anna", Sex.F, 31);
            Person.Wedding(alex, alexa);
            alex.SetChild(albert);
            alex.SetChild(sergey);
            sergey.SetChild(frank);
            Person.Wedding(anna, sergey);
            var alfred = new Person("Alfred", Sex.M, 13);
            anna.SetChild(alfred);

            
            Console.WriteLine(alfred.GetUncAlunt());
            Console.WriteLine(frank.GetCousin());
            Console.WriteLine(albert.GetParents());
            Console.ReadKey();
        }
    }
}
