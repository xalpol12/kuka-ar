package com.wawrzyniak.kukaComm.Service.RobotData;

import com.wawrzyniak.kukaComm.Repository.RobotStickerRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class RobotStickerService {

    private final RobotStickerRepository stickerRepository;

    @Autowired
    RobotStickerService(RobotStickerRepository repository) {
        this.stickerRepository = repository;
    }

    public Map<String, byte[]> getAllStickers() throws IOException {
        Map<String, byte[]> stickers = new HashMap<>();
        List<File> files = stickerRepository.getStickers();
        for (File file : files) {
            stickers.put(getBaseName(file.getName()), Files.readAllBytes(file.toPath()));
        }
        return stickers;
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
