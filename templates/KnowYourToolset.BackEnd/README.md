# KnowYourToolset.BackEnd

TODO: Update this text to contain a summary of this API - its general purpse, etc.

## Running the Solution

**PREREQUISITE:** [Seq](https://datalust.co/seq) is used as a destination to write log entries in addition to the Console -- they are much easier to read and explore via Seq.

The easiest way to run Seq locally is to run it as a Docker container:

```bash
docker pull datalust/seq
docker run -d --name seq -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
```

After this you should be able to see a user interface at [http://localhost:5341](http://localhost:5341)

Running the application will open a web browser to the Swagger user interface. Test away! See the [Instructions](Instructions.md#things-to-try) for some things to try :)

## Contributing

Provide instructions that should be followed for others to contribute to this repo/API.
