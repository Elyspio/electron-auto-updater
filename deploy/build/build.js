const { spawnSync } = require("child_process");
const path = require("path");

let dockerCommand =
  `docker buildx build --platform linux/amd64  -f ${__dirname}/Dockerfile  -t elyspio/electron-auto-updater --push .`
    .split(" ")
    .filter((str) => str.length);

spawnSync(dockerCommand[0], dockerCommand.slice(1), {
  cwd: path.resolve(__dirname, "../../"),
  stdio: "inherit",
});

spawnSync(
  "ssh",
  [
    "elyspio@192.168.0.59",
    "cd /apps/own/electron-auto-updater && docker-compose pull && docker-compose up -d",
  ],
  {
    cwd: __dirname,
    stdio: "inherit",
  }
);
