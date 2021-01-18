package ie.flipdish.challenge;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import org.json.JSONException;

import java.io.IOException;
import java.util.List;

import ie.flipdish.challenge.adapter.InsectRecyclerAdapter;
import ie.flipdish.challenge.models.Insect;
import ie.flipdish.challenge.utils.FileUtil;

public class MainActivity extends AppCompatActivity {

    private static final String EXTRA_INSECT_DETAILS = "insectExtra";
    private RecyclerView mRecyclerView;
    private InsectRecyclerAdapter mInsectRecyclerAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        mRecyclerView = findViewById(R.id.recycler_view);
        FileUtil util = new FileUtil(this);
        try {
            List<Insect> list = util.readInsectsFromResources();
            mInsectRecyclerAdapter = new InsectRecyclerAdapter(this, insect -> {
                Intent intent = new Intent(this, InsectDetailsActivity.class);
                intent.putExtra(EXTRA_INSECT_DETAILS, insect);
                startActivity(intent);
            });
            mInsectRecyclerAdapter.setData(list);
            mRecyclerView.setAdapter(mInsectRecyclerAdapter);
            mRecyclerView.setLayoutManager(new LinearLayoutManager(this));
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }
}