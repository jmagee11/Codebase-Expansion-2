/* eslint-disable indent */
/* eslint-disable max-len */

const functions = require("firebase-functions");

const admin = require("firebase-admin");
admin.initializeApp();

const cors = require("cors");
const express = require("express");
const bodyParser = require("body-parser");

const app = express();
const jsonParser = bodyParser.json();

app.use(cors({origin: true}));

// Take a new high score and insert it into the score list
app.post("/highscores", jsonParser, async (req, res) => {
    console.log(JSON.stringify(req.body));

    const highscore = {
      name: req.body.name,
      value: parseInt(req.body.value, 10),
      timestamp: admin.firestore.Timestamp.now(),
    };

    // eslint-disable-next-line max-len
    if (isNaN(highscore.value) || !(typeof highscore.name === "string" || highscore.name instanceof String)) {
      res.status(422).json({info: "Invalid body"});
    } else if (highscore.name.length < 4 || highscore.name.length > 16) {
      res.status(422).json({info: "'name'.length < 4 || 'name'.length > 16"});
    } else {
      await admin.firestore()
        .collection("demos/ggj22/highscores")
        .add(highscore);

      res.json({
        data: highscore,
        info:
          "Dear cheater and bane of 'nice to have things',\n" +
          "this is a GameJam product and meant to bring joy to everybody who enjoys it.\n" +
          "Pls be kind and dont screw with the highscore list by cheating or sending post request manually.\n" +
          "I know it is easy to do, but you have to understand, that you just make people sad.\n" +
          "People who enjoy features like a highscore list and a healthy competition.\n" +
          "Thanks for understanding and may you brighten at least somebodys day someday.\n" +
          "Sincerely Claas\n" +
          "p.s. dont feel to bad. Today could be the day you change.",
      });
    }
  },
);

// Return the top 10 highscores ordered
app.get("/highscores", async (req, res) => {
  res.json({
    items: (await admin.firestore()
      .collection("demos/ggj22/highscores")
      .orderBy("value", "desc")
      .orderBy("timestamp", "asc")
      .limit(10)
      .get()).docs.map((d) => d.data()),
  });
});

exports.ggj22 = functions
  .region("europe-west1")
  .https.onRequest(app);
