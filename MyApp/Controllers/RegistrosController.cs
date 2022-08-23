using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using MyApp.Data;
using MyApp.DataSet;
using MyApp.Models;

namespace MyApp.Controllers
{

    public class RegistrosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private object _webHostEnvironment;

        public RegistrosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Registros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Registro.ToListAsync());
        }

        // GET: Registros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registros = await _context.Registro
                .FirstOrDefaultAsync(m => m.IdRegistro == id);
            if (registros == null)
            {
                return NotFound();
            }

            return View(registros);
        }

        // GET: Registros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
        Create([Bind("IdRegistro,Imagen,Documento,Nombre,Apellidos,FechaNac,Direccion,Celular,Genero,Deporte,Trabaja,Sueldo,Estado")] Registros registros, IFormFile ImageFile)
        {
            //if (ModelState.IsValid)
            if (ImageFile != null && ImageFile.Length > 0)
            {
                registros.Imagen = GetByteArrayFromImage(ImageFile);
                _context.Add(registros);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
            }
            return View(registros);
        }


        // GET: Registros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registros = await _context.Registro.FindAsync(id);
            if (registros == null)
            {
                return NotFound();
            }
            return View(registros);
        }

        // POST: Registros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
 [Bind("IdRegistro,Imagen,Documento,Nombre,Apellidos,FechaNac,Direccion,Celular,Genero,Deporte,Trabaja,Sueldo,Estado")] Registros registros, IFormFile ImageFile)
  {
            if (id != registros.IdRegistro)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if ((ImageFile != null) || (registros.Imagen.Length > 0))
                {
                    try
                    {
                        if (ImageFile != null && ImageFile.Length > 0)
                        {
                            registros.Imagen = GetByteArrayFromImage(ImageFile);                         
                        }
                        _context.Update(registros);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RegistrosExists(registros.IdRegistro))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return View(registros);
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }


        // GET: Registros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registros = await _context.Registro
                .FirstOrDefaultAsync(m => m.IdRegistro == id);
            if (registros == null)
            {
                return NotFound();
            }

            return View(registros);
        }

        // POST: Registros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registros = await _context.Registro.FindAsync(id);
            _context.Registro.Remove(registros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrosExists(int id)
        {
            return _context.Registro.Any(e => e.IdRegistro == id);
        }

        public IActionResult Imprimir()
        {
            string renderFormat = "PDF";
            string mimetype = "application/PDF";
            using var report = new LocalReport();
            // Ruta donde está el archivo rdcl
            report.ReportPath =
           $"{this._webHostEnvironment.WebRootPath}\\Reportes\\ReportGeneral.rdlc";
            DataSet1 ds = new DataSet1();
            DataSet.DataSet1TableAdapters.RegistroTableAdapter sda = new
           DataSet.DataSet1TableAdapters.RegistroTableAdapter();
            sda.Fill(ds.Registro);
            ReportDataSource rds = new ReportDataSource("DataSetReportes", (object)ds.Registro);
            report.DataSources.Add(rds);
            //report.Refresh();
            // Convierte el archivo en binario
            var pdf = report.Render(renderFormat);
            report.Refresh();
            // Descarga el documento en un archivo pdf
            //return File(pdf,mimetype,"report." + "pdf");
            // Muestra el reporte en la pantalla
            return File(pdf, mimetype);
        }
        public IActionResult DescargarPdf()
        {
            string renderFormat = "PDF";
            string mimetype = "application/PDF";
            using var report = new LocalReport();
            // Ruta donde está el archivo rdcl
            report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Reportes\\ReportGeneral.rdlc";
            DataSet1 ds = new DataSet1();
            DataSet.DataSet1TableAdapters.RegistroTableAdapter sda = new DataSet.DataSet1TableAdapters.RegistroTableAdapter();
            sda.Fill(ds.Registro);
            ReportDataSource rds = new ReportDataSource("DataSetReportes", (object)ds.Registro);
            report.DataSources.Add(rds);
            report.Refresh();
            // Convierte el archivo en binario
            var pdf = report.Render(renderFormat);
            // Descarga el documento en un archivo pdf
            return File(pdf, mimetype, "report." + "pdf");
            // Muestra el reporte en la pantalla
            //return File(pdf, mimetype);
        }

    }

}
