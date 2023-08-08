package com.wawrzyniak.kukaComm.Repository;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Repository;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

@Repository
public class RobotModelRepository {

    @Value("${desc.location}")
    private String location;

    public List<File> getDescription() {
        File dir = new File(location);
        File[] files = dir.listFiles((dir1, name) -> name.endsWith(".json"));
        if (files == null) {
            return new ArrayList<>();
        }
        return new ArrayList<>(List.of(files));
    }
}
