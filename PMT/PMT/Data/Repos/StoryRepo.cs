﻿using Microsoft.EntityFrameworkCore;
using PMT.Data.RepoInterfaces;
using PMT.Services;

namespace PMT.Data.Models;

public class StoryRepo : IStoryRepo
{
  private readonly AppDbContext _db;

  public StoryRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(Story story)
  {
    _db.Stories.Add(story);
    return Save();
  }

  public bool Delete(Story story)
  {
    _db.Stories.Remove(story);
    return Save();
  }

  public async Task<IEnumerable<Story>> GetAllFromUserAsync(int projId, string appUserId)
  {
    // add specific user when the AssignedTo Property is added
    var storiesFromProj = await _db.Stories.Where(s => s.ProjId == projId).ToListAsync();
    return storiesFromProj;
  }

  public async Task<IEnumerable<Story>> GetAllUnresolvedStoriesFromUserAsync(int projId, string appUserId)
  {
    var storiesFromProj = await _db.Stories.Where(s => s.ProjId == projId).ToListAsync();
    return storiesFromProj.Where(s => s.Status != Str.Resolved);
  }

  public async Task<IEnumerable<Story>> GetAllUnresolvedStoriesWithSearchFilterAsync(int projId, string filterString)
  {
    filterString ??= string.Empty;
    filterString = filterString.ToUpper();

    var unresolvedStoriesFromProj = _db.Stories.Where(s => s.ProjId == projId && s.Status != Str.Resolved);
    unresolvedStoriesFromProj = unresolvedStoriesFromProj.Where(s => s.Title.ToUpper().Contains(filterString));
    return await unresolvedStoriesFromProj.ToListAsync();
  }

  public async Task<IEnumerable<Story>> GetAllWithSearchFilterAsync(int projId, string filterString)
  {
    filterString ??= string.Empty;
    filterString = filterString.ToUpper();

    var storiesFromProj = _db.Stories.Where(s => s.ProjId == projId);
    storiesFromProj = storiesFromProj.Where(s => s.Title.ToUpper().Contains(filterString));
    return await storiesFromProj.ToListAsync();
    //return storiesFromProj.Where(s => s.Title.ToUpper().Contains(filterString)); ViewData was coming up null
  }

  public async Task<IEnumerable<Story>> GetAllResolved(int projId)
  {
    var stories = _db.Stories.Where(s => s.ProjId == projId);
    stories = stories.Where(s => s.Status == Str.Resolved);
    return await stories.ToListAsync();
  }

  public async Task<Story> GetByIdAsync(int id)
  {
    return await _db.Stories.FindAsync(id);
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }

  public bool Update(Story story)
  {
    _db.Stories.Update(story);
    return Save();
  }
}
