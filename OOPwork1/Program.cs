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
        private int childID { get; set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public Sex Sex { get; private set; }
        public Person Mother { get; private set; }
        public Person Father { get; private set; }
        private List<Person> Child { get; set; }
        private Person maritalUnion { get; set; }

        public Person(string name, Sex sex, int age)
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
                if (mother.Age <= Age)
                    throw new ArgumentException("Мать не может быть младше ребенка или равной ей по возрасту");
            }
            if (father != null)
            {
                if (father.Sex == Sex.M && father.Age > Age)
                    Father = father;
                else
                    if (father.Sex != Sex.M)
                    throw new ArgumentException(String.Format("Не тот пол"));
                if (father.Age <= Age)
                    throw new ArgumentException("Отец не может быть младше ребенка или равным ему по возрасту");
            }

        }
        public Person[] GetParents() => new Person[] { Mother, Father };
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
        public Person[] GetChild() => Child.ToArray();

        public static void Wedding(Person mUnion1, Person mUnion2)
        {
            if (mUnion1.maritalUnion != null)
                mUnion1.maritalUnion.maritalUnion = null;
            mUnion1.maritalUnion = mUnion2;

            if (mUnion2.maritalUnion != null)
                mUnion2.maritalUnion.maritalUnion = null;
            mUnion2.maritalUnion = mUnion1;
        }

        private List<Person> GetSistersAndBrothersParent(Person grandma, Person grandpa) => grandma.Child.Union(grandpa.Child).ToList();

        public Person[] GetCousin()
        {
            var ans = new List<Person>();
            if (Mother != null)
            {
                var motherSistersAndBrothers = GetSistersAndBrothersParent(Mother.Mother, Mother.Father);
                foreach (var mSAB in motherSistersAndBrothers)
                {
                    if (mSAB.childID == Mother.childID)
                        continue;
                    foreach (var cousin in mSAB.Child)
                        ans.Add(cousin);
                }
            }
            if (Father != null)
            {
                var fatherSistersAndBrothers = GetSistersAndBrothersParent(Father.Mother, Father.Father);
                foreach (var mSAB in fatherSistersAndBrothers)
                {
                    if (mSAB.childID == Father.childID)
                        continue;
                    foreach (var cousin in mSAB.Child)
                        ans.Add(cousin);
                }
            }

            return ans.ToArray();
        }
        public Person[] GetUncAlunt()
        {
            var ans = new List<Person>();
            if (Mother.Mother != null && Mother.Father != null)
            {
                var motherSistersAndBrothers = GetSistersAndBrothersParent(Mother.Mother, Mother.Father);
                foreach (var mSAB in motherSistersAndBrothers)
                {
                    if (mSAB.childID == Mother.childID)
                        continue;
                    ans.Add(mSAB);
                    if (mSAB.maritalUnion != null)
                        ans.Add(mSAB.maritalUnion);
                }
            }
            if (Father.Mother != null && Father.Father != null)
            {
                var fatherSistersAndBrothers = GetSistersAndBrothersParent(Father.Mother, Father.Father);
                foreach (var mSAB in fatherSistersAndBrothers)
                {
                    if (mSAB.childID == Mother.childID)
                        continue;
                    ans.Add(mSAB);
                }
            }
            return ans.ToArray();
        }

        public Person[] GetInLaw() => maritalUnion != null? new Person[] { maritalUnion.Father, maritalUnion.Mother }: Array.Empty<Person>();


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

            var alfredUncAlunt = alfred.GetUncAlunt();
            Console.WriteLine($"Дяди и тети {alfred.Name}:");
            foreach (var unc in alfredUncAlunt)
            {
                Console.WriteLine($"\t{unc.Name}");
            }
            Console.WriteLine(frank.GetCousin());
            var albertParents = albert.GetParents();
            Console.WriteLine($"Родители {albert.Name}:");
            foreach (var parent in albertParents)
                Console.WriteLine($"Name = {parent.Name}, Sex = {parent.Sex}");
            Console.ReadKey();
        }
    }
}
