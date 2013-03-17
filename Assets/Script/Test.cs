using UnityEngine;
using System;
using System.Linq;
using GDataDB;
using GDataDB.Linq;

public class Test : MonoBehaviour
{
	void Start()
	{
		Debug.Log("Connecting");

		const string account = "account@gmail.com";
		const string password = "password";
		var client = new DatabaseClient(account, password);

		const string dbName = "spreadsheet-test";
		var db = client.GetDatabase(dbName) ?? client.CreateDatabase(dbName); 	

		const string tableName = "test_table";
		var t = db.GetTable<Entity>(tableName) ?? db.CreateTable<Entity>(tableName);

		Debug.Log("Feed url for this table is : " + t.GetFeedUrl());

		var all = t.FindAll();

		var r = all.Count > 0 ? all[0] : t.Add(new Entity {Conteudo = "some content", Amount = 5});
		r.Element.Conteudo = "nothing at all";
		r.Update();	


		Debug.Log("Now there are " + t.FindAll().Count + "elements");
		{
			Debug.Log("executing a few queries");
			foreach (var q in new[] { "amount=5", "amount<6", "amount>0", "amount!=6", "amount<>6", "conteudo=\"nothing at all\"" }) {
				Debug.Log("querying: " + q);
				var rows = t.FindStructured(q);
				Debug.Log("elements found: " + rows.Count);
			}
		}
		{
			Debug.Log("Linq queries");
			var rows = from e in t.AsQueryable()
								 where e.Conteudo == r.Element.Conteudo
								 orderby e.Amount
								 select e;
			Debug.Log("elements found : " + rows.ToList().Count());
		}

		//r.Delete();
		//t.Delete();
		//db.Delete();

	}
}