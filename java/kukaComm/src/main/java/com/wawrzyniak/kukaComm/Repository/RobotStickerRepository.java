package com.wawrzyniak.kukaComm.Repository;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Repository;

import java.io.File;
import java.io.FilenameFilter;
import java.util.ArrayList;
import java.util.List;

@Repository
public class RobotStickerRepository {

    @Value("${img.location}")
    private String location;
    private final FilenameFilter fileFilter;

    RobotStickerRepository(){
        fileFilter = new ImageFileFilter(".png", ".jpg");
    }

    public List<File> getStickers() {
        File dir = new File(location);
        File[] files = dir.listFiles(fileFilter);
        if (files == null) {
            return new ArrayList<>();
        }
        return new ArrayList<>(List.of(files));
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
