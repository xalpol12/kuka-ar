package com.wawrzyniak.kukaComm.Service.RobotData;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.wawrzyniak.kukaComm.Model.ModelReading.RobotData;
import com.wawrzyniak.kukaComm.Repository.RobotModelRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.io.File;
import java.io.IOException;
import java.util.*;

@Service
public class RobotModelService {

    private final RobotModelRepository robotRepository;
    private final ObjectMapper mapper = new ObjectMapper();

    @Autowired
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
    public RobotModelService(RobotModelRepository repository) {
=======
    public RobotModelService(RobotModelRepository repository){
>>>>>>> add testSocket and kukaComm
=======
    public RobotModelService(RobotModelRepository repository) {
>>>>>>> add swagger docs, fix some whitespace issues
        robotRepository = repository;
    }

    public Map<String, Map<String, RobotData>> getAvailableRobots() throws IOException {
        Map<String, Map<String, RobotData>> robots =  new HashMap<>();
        List<File> files = robotRepository.getDescription();
        for (File description : files) {
            RobotData[] set = mapper.readValue(description, RobotData[].class);
            Map<String, RobotData> temp = new HashMap<>();
            for(RobotData robot : set){
                temp.put(robot.name(), robot);
            }
            robots.put(getBaseName(description.getName()), temp);
        }
        return robots;
    }

    private String getBaseName(String fileName) {
        int index = fileName.lastIndexOf('.');
        if (index == -1) {
            return fileName;
        } else {
            return fileName.substring(0, index);
        }
    }

}
