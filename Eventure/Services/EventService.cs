using Microsoft.EntityFrameworkCore;
using Eventure.Data;
using Eventure.Models;
using Eventure.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Eventure.Services;

public class EventService : IEventService
{
    private readonly DatabaseContext _context;

    public EventService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EventComponent>> GetAllAsync() =>
        await _context.Events.ToListAsync();

    public async Task<EventComponent?> GetByIdAsync(int id) =>
        await _context.Events.FindAsync(id);

    public async Task<EventComponent> CreateAsync(EventComponent ev)
    {
        _context.Events.Add(ev);
        await _context.SaveChangesAsync();
        return ev;
    }

    public async Task<EventComponent?> UpdateAsync(int id, EventComponent ev)
    {
        var existing = await _context.Events.FindAsync(id);
        if (existing == null) return null;

        existing.Title = ev.Title;
        existing.Description = ev.Description;
        existing.Date = ev.Date;
        existing.IsPublic = ev.IsPublic;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Events.FindAsync(id);
        if (existing == null) return false;

        _context.Events.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}