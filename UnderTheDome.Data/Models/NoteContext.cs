using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using UnderTheDome.Data.Interfaces;

namespace UnderTheDome.Data.Models
{
    public class NoteContext : INoteContext
    {
        private string _filePath;
        
        public NoteContext()
        {
            _filePath = Directory.GetCurrentDirectory() + @"\actorNotes.txt";
            if (!File.Exists(_filePath))
                using (StreamWriter sw = new StreamWriter(_filePath)) { sw.Write(@"[]"); };
        }

        public bool AddNoteForActor(Note note)
        {
            string fileContent;
            bool noteIsOverwritten;
            List<Note> notes;

            using (StreamReader sr = new StreamReader(_filePath))
            {
                fileContent = sr.ReadToEnd();
            }
            notes = JsonSerializer.Deserialize<List<Note>>(fileContent);

            Note someNote = notes.Find(x => x.ActorId == note.ActorId);
            if (someNote != null)
            {
                someNote.ActorNote = note.ActorNote;
                noteIsOverwritten = true;
            }
            else
            {
                notes.Add(note);
                noteIsOverwritten = false;
            }

            fileContent = JsonSerializer.Serialize(notes);

            using (StreamWriter sw = new StreamWriter(_filePath))
            {
                sw.Write(fileContent);
            }

            return noteIsOverwritten;
        }

        public Note GetNoteForActor(int actorId)
        {
            string fileContent;
            List<Note> notes;

            using (StreamReader sr = new StreamReader(_filePath))
            {
                fileContent = sr.ReadToEnd();
            }
            notes = JsonSerializer.Deserialize<List<Note>>(fileContent);

            return notes.Find(x => x.ActorId == actorId);
        }

        public void RemoveNoteFromFile(Note note)
        {
            string fileContent;
            List<Note> notes;

            using (StreamReader sr = new StreamReader(_filePath))
            {
                fileContent = sr.ReadToEnd();
            }
            notes = JsonSerializer.Deserialize<List<Note>>(fileContent);

            Note someNote = notes.Find(x => x.ActorId == note.ActorId);
            notes.Remove(someNote);

            fileContent = JsonSerializer.Serialize(notes);

            using (StreamWriter sw = new StreamWriter(_filePath))
            {
                sw.Write(fileContent);
            }
        }
    }
}
