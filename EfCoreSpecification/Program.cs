using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace EfCoreSpecification
{
    class Program
    {
        static KisiContext db = new KisiContext();

        static async Task Main(string[] args)
        {
            db.Database.EnsureCreated();

            //isminde e harfi geçen alfabetik olarak sıralayıp ilk 3 kişiyi al
            //var sorgu = db.Kisiler
            //    .Where(x => x.Ad.Contains("e"))
            //    .OrderBy(x => x.Ad)
            //    .Take(3);

            //var kisiler = KisileriGetir(x => x.Ad.Contains("e"), x => x.Ad, 3);

            var kisiler =await KisileriGetir(new KisiFiltre());

            foreach (var item in kisiler) 
            {

                Console.WriteLine(item.Ad);
            }


            Console.ReadKey();
        }

        static async Task<List<Kisi>> KisileriGetir(ISpecification<Kisi> specification)
        {
            return (await EfSpecificationEvaluator<Kisi>.GetQuery(db.Kisiler, specification)).ToList();
        }

        //static List<Kisi> KisileriGetir(Expression<Func<Kisi, bool>> where, Expression<Func<Kisi, string>> orderBy, int take)
        //{
        //    return db.Kisiler.Where(where).OrderBy(orderBy).Take(take).ToList();
        //}
    }

    public class KisiFiltre : BaseSpecification<Kisi>
    {
        public KisiFiltre()
        {
            AddCriteria(x=>x.Ad.Contains("e"));
            ApplyOrderBy(x => x.Ad);
            ApplyPaging(0,3);
        }
    }
   
    public class Kisi
    {
        public int Id { get; set; }
        public string Ad { get; set; }
    }

    public class KisiContext : DbContext
    {
        public DbSet<Kisi> Kisiler { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=(localdb)\mssqllocaldb;initial catalog=KisiDb16Haz;integrated security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Kisi>().HasData(
                new Kisi { Id = 1, Ad = "Ali" },
                new Kisi { Id = 2, Ad = "Veli" },
                new Kisi { Id = 3, Ad = "Can" },
                new Kisi { Id = 4, Ad = "Cem" },
                new Kisi { Id = 5, Ad = "Ayşe" },
                new Kisi { Id = 6, Ad = "Arzu" },
                new Kisi { Id = 7, Ad = "Emel" },
                new Kisi { Id = 8, Ad = "Nur" }
            );
        }
    }
}
    