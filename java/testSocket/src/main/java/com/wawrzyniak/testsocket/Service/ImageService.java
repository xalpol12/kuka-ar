package com.wawrzyniak.testsocket.Service;

import org.springframework.stereotype.Service;

import java.io.File;
import java.io.FilenameFilter;
import java.io.IOException;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class ImageService {

    private String location = "Images/";

    private final FilenameFilter fileFilter;

    ImageService(){
        fileFilter = new ImageFileFilter(".png", ".jpg");
    }

    private List<File> getStickers() {
        File dir = new File(location);
        File[] files = dir.listFiles(fileFilter);
        if (files == null) {
            return new ArrayList<>();
        }
        return new ArrayList<>(List.of(files));
    }

    public Map<String, byte[]> getAllStickers() throws IOException {
        Map<String, byte[]> stickers = new HashMap<>();
        List<File> files = getStickers();
        for (File file : files){
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
    private class ImageFileFilter implements FilenameFilter {

        private final String[] exts;

        ImageFileFilter(String... extensions){
            this.exts = extensions;
        }

        @Override
        public boolean accept(File dir, String name) {
            for(String ext : exts){
                if (name.endsWith(ext)){
                    return true;
                }
            }
            return false;
        }
    }

}
