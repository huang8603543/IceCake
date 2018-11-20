using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using IceCake.Core.Json;

public class GameApplication : MonoBehaviour
{
	void Start ()
    {
        string jsonStr = TestJsonSerialize();
        TestJsonDeserialize(jsonStr);
    }

    string TestJsonSerialize()
    {
        Person p1 = new Person() { Name = "hzc", Age = 28 };
        Person p2 = new Person() { Name = "jyt", Age = 21 };
        Team team = new Team() { Name = "ice" };
        team.People.Add(p1);
        team.People.Add(p2);
        JsonNode jsonNode = JsonParser.ToJsonNode(team);
        Debug.Log(jsonNode.ToString());
        return jsonNode.ToString();
    }

    void TestJsonDeserialize(string jsonStr)
    {
        JsonNode jsonNode = JsonParser.Parse(jsonStr);
        Team team = jsonNode.ToObject<Team>();
        Debug.Log(team.ToString());
    }
}

[Serializable]
public class Person
{
    public string Name;
    public int Age;

    public override string ToString()
    {
        return "Name: " + Name + " Age: " + Age;
    }
}

[Serializable]
public class Team
{
    public string Name;
    public List<Person> People = new List<Person>();

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("Name: " + Name + " People: ");
        foreach (var person in People)
        {
            sb.Append(person.ToString());
        }
        return sb.ToString();
    }
}
