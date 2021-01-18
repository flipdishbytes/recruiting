package ie.flipdish.challenge.utils;

import android.content.Context;
import android.content.res.Resources;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

import ie.flipdish.challenge.R;
import ie.flipdish.challenge.models.Insect;

public class FileUtil {

    private static final String INSECTS = "insects";
    public static final String FRIENDLY_NAME = "friendlyName";
    public static final String SCIENTIFIC_NAME = "scientificName";
    public static final String CLASSIFICATION = "classification";
    public static final String IMAGE_ASSET = "imageAsset";
    public static final String DANGER_LEVEL = "dangerLevel";

    private Resources mResources;

    public FileUtil(Context context) {
        this.mResources = context.getResources();
    }

    public List<Insect> readInsectsFromResources() throws IOException, JSONException {
        StringBuilder builder = new StringBuilder();
        InputStream in = mResources.openRawResource(R.raw.insects);
        BufferedReader reader = new BufferedReader(new InputStreamReader(in));

        String line;
        while ((line = reader.readLine()) != null) {
            builder.append(line);
        }

        final String rawJson = builder.toString();

        JSONObject jsonObject = new JSONObject(rawJson);
        JSONArray jsonArray = jsonObject.getJSONArray(INSECTS);
        List<Insect> insectList = new ArrayList<>();

        for (int i = 0; i < jsonArray.length(); i++) {
            Insect insect = new Insect(
                    jsonArray.getJSONObject(i).getString(FRIENDLY_NAME),
                    jsonArray.getJSONObject(i).getString(SCIENTIFIC_NAME),
                    jsonArray.getJSONObject(i).getString(CLASSIFICATION),
                    jsonArray.getJSONObject(i).getString(IMAGE_ASSET),
                    Integer.parseInt(jsonArray.getJSONObject(i).getString(DANGER_LEVEL)));
            insectList.add(insect);
        }

        return insectList;
    }
}
