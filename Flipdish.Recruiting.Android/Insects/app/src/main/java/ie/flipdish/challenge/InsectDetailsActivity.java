package ie.flipdish.challenge;

import android.content.res.Resources;
import android.graphics.drawable.Drawable;
import android.os.Bundle;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;

import android.widget.ImageView;
import android.widget.RatingBar;
import android.widget.TextView;
import java.io.IOException;
import java.io.InputStream;
import ie.flipdish.challenge.models.Insect;

public class InsectDetailsActivity extends AppCompatActivity {

    private static final String EXTRA_INSECT_DETAILS = "insectExtra";
    private Insect mInsectDetails;
    private ImageView mInsectImageView;
    private static TextView mCommonName;
    private static TextView mScientificName;
    private TextView mClassification;
    private RatingBar mRatingBar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_insect_details);
        setupActionBar();
        mInsectImageView = (ImageView) findViewById(R.id.insectImage);
        mCommonName  = (TextView) findViewById(R.id.commonNameView);
        mScientificName = (TextView) findViewById(R.id.scientificNameView);
        mClassification = (TextView)findViewById(R.id.classification);
        mRatingBar  = (RatingBar) findViewById(R.id.rating);
        setCellValues();
    }

    public void setupActionBar(){
        ActionBar actionBar = getSupportActionBar();
        if(actionBar != null) {
            actionBar.setDisplayHomeAsUpEnabled(true);
        }
    }

    public void setCellValues(){
        getInsectDetails();
        setInsectImage(mInsectDetails.imageAsset);
        mCommonName.setText(mInsectDetails.name);
        mScientificName.setText(mInsectDetails.scientificName);
        setClassification(mInsectDetails.classification);
        mRatingBar.setRating((float)mInsectDetails.dangerLevel);
    }

    public void getInsectDetails() {
        if(getIntent().getParcelableExtra(EXTRA_INSECT_DETAILS) != null ) {
            mInsectDetails = getIntent().getParcelableExtra(EXTRA_INSECT_DETAILS);
        }
    }

    public void setInsectImage(final String imageName) {
        InputStream ims;
        try {
            ims = getAssets().open(imageName);
            mInsectImageView.setImageDrawable(Drawable.createFromStream(ims, null));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void setClassification(final String classification) {
        Resources res = getResources();
        String text = String.format(res.getString(R.string.classification), classification);
        mClassification.setText(text);
    }
}