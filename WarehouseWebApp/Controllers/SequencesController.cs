using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseWebApp.Models;

namespace WarehouseWebApp.Controllers
{
    public class SequencesController
    {
        private readonly dbwarehouseContext _context;
        public SequencesController()
        {
            _context = new dbwarehouseContext();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Edit(Sequence sequence)
        {


            try
            {
                sequence.SeqValue = sequence.SeqValue + 1;
                _context.Update(sequence);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SequenceExists(sequence.SeqName))
                {
                }
                else
                {
                    throw;
                }
            }
        }



        public async Task<Sequence> GetSequence(string seqName)
        {
            var sequence = await _context.Sequences.FirstOrDefaultAsync(x => x.SeqName == seqName);
            return sequence;
        }

        // POST: Sequences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create(Sequence sequence)
        {

                _context.Add(sequence);
                await _context.SaveChangesAsync();
            
        }


        // POST: Sequences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task DeleteConfirmed(string id)
        {
            if (_context.Sequences == null)
            {
                return;
            }
            var sequence = await _context.Sequences.FindAsync(id);
            if (sequence != null)
            {
                _context.Sequences.Remove(sequence);
            }

            await _context.SaveChangesAsync();
        }

        private bool SequenceExists(string id)
        {
            return _context.Sequences.Any(e => e.SeqName == id);
        }
    }
}
