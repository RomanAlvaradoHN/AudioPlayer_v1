using SQLite;
using AudioPlayer_v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer_v1.Controllers {
    public class DBController {

        //DATABASE CONFIGURATION VARIABLES
        //=======================================================================================
        private readonly static string dbFileName = "MyAppDB.db3";

        private readonly SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite |
                                                 SQLiteOpenFlags.Create |
                                                 SQLiteOpenFlags.SharedCache;

        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);
        //---------------------------------------------------------------------------------------
        private SQLiteAsyncConnection connection;
        //======================================================================================


        public DBController() {
        }



        private async Task Init() {
            if (connection is not null) {
                return;
            }
            connection = new SQLiteAsyncConnection(dbPath, flags);
            await connection.CreateTableAsync<Audios>();
        }



        //CREATE ==============================================================
        public async Task<int> Insert(Audios registro) {
            await Init();
            return await connection.InsertAsync(registro);
        }




        //READ ==============================================================
        public async Task<List<Audios>> SelectAll() {
            await Init();
            return await connection.Table<Audios>().ToListAsync();
        }


        public async Task<Audios> SelectById(int id) {
            await Init();
            return await connection.Table<Audios>().Where(col => col.Id == id).FirstOrDefaultAsync();
        }




        //UPDATE ==============================================================
        public async Task<int> Update(Audios registro) {
            await Init();
            return await connection.UpdateAsync(registro);
        }




        //DELETE ==============================================================
        public async Task<int> Delete(Audios registro) {
            await Init();
            return await connection.DeleteAsync(registro);
        }
    }
}