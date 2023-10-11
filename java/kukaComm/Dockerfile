# JAVA SETUP
FROM openjdk:17
LABEL maintainer="Coyot Willy"

# FOLDER STRUCTURE CREATION
VOLUME /kukaComm
WORKDIR /kukaComm/build/java
COPY RobotData /kukaComm/build/RobotData
COPY RobotStickers /kukaComm/build/RobotStickers

# JAR FILE MOVE
ARG JAR_FILE=target/*.jar
COPY ${JAR_FILE} app.jar

# RUN SERVER
ENTRYPOINT ["java","-jar","app.jar"]