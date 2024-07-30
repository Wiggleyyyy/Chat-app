const mariadb = require("../../../../node_modules/mariadb");

async function createConnection() {
    let conn;
    try {
        conn = await mariadb.createConnection({
            host: "localhost",
            port: "al01_skp-dp_sde_dk",
            user: "al01_skp-dp_sde_dk",
            password: "5yz52pqz",
        });

        console.log("Connected to database!");
    } catch (err) {
        console.log(err);
    } finally {
        if (conn) {
            await conn.close();
        }
    }
}